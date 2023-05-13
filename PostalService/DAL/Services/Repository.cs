using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Core.Models;
using DAL.Abstractions;

namespace DAL.Services
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly string _filePath;

        public Repository(string filePath = null)
        {
            _filePath = filePath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{typeof(T).Name}.json");
            EnsureFileExists();
        }

        public async Task<Result<List<T>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var items = await GetAllItemsAsync();
                var pagedItems = items
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new Result<List<T>>(isSuccessful: true, data: pagedItems);
            }
            catch (Exception ex)
            {
                return new Result<List<T>>(isSuccessful: false, message: $"Failed to get all items. Exception: {ex.Message}");
            }
        }

        public async Task<Result<T>> GetByIdAsync(Guid id)
        {
            return await GetByPredicateAsync(item => item.Id.Equals(id));
        }

        public async Task<Result<T>> GetByPredicateAsync(Func<T, bool> predicate)
        {
            try
            {
                var item = (await GetAllItemsAsync()).FirstOrDefault(predicate);

                if (item == null)
                {
                    return new Result<T>(isSuccessful: false, message: "Item not found.");
                }

                return new Result<T>(isSuccessful: true, data: item);
            }
            catch (Exception ex)
            {
                return new Result<T>(isSuccessful: false, message: $"Failed to get item. Exception: {ex.Message}");
            }
        }

        public async Task<Result<bool>> AddAsync(T obj)
        {
            try
            {
                var items = await GetAllItemsAsync();
                items.Add(obj);
                await SaveItemsAsync(items);

                return new Result<bool>(isSuccessful: true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(isSuccessful: false, message: $"Failed to add item. Exception: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateAsync(Guid id, T updatedObj)
        {
            try
            {
                var items = await GetAllItemsAsync();
                int index = items.FindIndex(item => item.Id.Equals(id));

                if (index != -1)
                {
                    items[index] = updatedObj;
                    await SaveItemsAsync(items);

                    return new Result<bool>(isSuccessful: true);
                }

                return new Result<bool>(isSuccessful: false, message: "Object with the specified Id not found.");
            }
            catch (Exception ex)
            {
                return new Result<bool>(isSuccessful: false, message: $"Failed to update item. Exception: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var items = await GetAllItemsAsync();
                int index = items.FindIndex(item => item.Id.Equals(id));

                if (index != -1)
                {
                    items.RemoveAt(index);
                    await SaveItemsAsync(items);

                    return new Result<bool>(isSuccessful: true);
                }

                return new Result<bool>(isSuccessful: false, message: "Object with the specified Id not found.");
            }
            catch (Exception ex)
            {
                return new Result<bool>(isSuccessful: false, message: $"Failed to delete item. Exception: {ex.Message}");
            }
        }

        public async Task<Result<string>> PackAllToPdf()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");

            var fileNumber = 1;
            var fileName = $"{typeof(T).Name}s[{date}].pdf";
            
            while (File.Exists(fileName))
            {
                fileName = $"{typeof(T).Name}s[{date}]({fileNumber}).pdf";
                fileNumber++;
            }

            var document = new Document(PageSize.A4, 50, 50, 50, 50);

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

            document.Open();

            var items = await GetAllItemsAsync();
            
            if (items.Count == 0)
            {
                return new Result<string>(isSuccessful: false, message: "No items to pack.");
            }

            var elemNumber = 1;
            
            foreach (var item in items)
            {
                var text = item.ToString();
                
                var caption = new Paragraph($"{typeof(T).Name} {elemNumber++}\n\n", new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD))
                    {
                        Alignment = Element.ALIGN_CENTER
                    };

                var paragraph = new Paragraph(text);

                document.Add(caption);
                document.Add(paragraph);

                BarcodeQRCode qrcode = new BarcodeQRCode(text, 150, 150, null);

                Image image = qrcode.GetImage();

                image.SetAbsolutePosition((document.PageSize.Width - image.ScaledWidth) / 2, document.Bottom + 30);

                document.Add(image);
                document.NewPage();
            }

            document.Close();
            
            return new Result<string>(isSuccessful: true, data: fileName);
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                WriteToFileAsync(new List<T>()).GetAwaiter().GetResult();
            }
        }

        private async Task<List<T>> GetAllItemsAsync()
        {
            try
            {
                using StreamReader file = File.OpenText(_filePath);
                using JsonTextReader reader = new JsonTextReader(file);
                JsonSerializer serializer = new JsonSerializer();
                
                return await Task.Run(() => serializer.Deserialize<List<T>>(reader));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get items from the file. Exception: {ex.Message}");
            }
        }

        private async Task WriteToFileAsync(List<T> items)
        {
            try
            {
                using StreamWriter file = File.CreateText(_filePath);
                using JsonTextWriter writer = new JsonTextWriter(file)
                {
                    Formatting = Formatting.Indented
                };
                
                JsonSerializer serializer = new JsonSerializer();
                await Task.Run(() => serializer.Serialize(writer, items));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write items to the file. Exception: {ex.Message}");
            }
        }

        private async Task SaveItemsAsync(List<T> items)
        {
            await WriteToFileAsync(items);
        }
    }
}