using IronXL;
using Microsoft.AspNetCore.Http;

namespace Competitions.Common.Helpers
{
    public static class FileConvertor
    {
        public static byte[] ReadBytes(this IFormFile file)
        {
            byte[] obj = null;
            using (var fileStream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    fileStream.CopyTo(memoryStream);
                    obj = memoryStream.ToArray();
                }
            }
            return obj;
        }

        public static void CreateExcel(List<List<String>> data, String path, bool haveHeader = false)
        {
            WorkBook workBook = WorkBook.Create(ExcelFileFormat.XLSX);
            var workSheet = workBook.CreateWorkSheet("students");

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    var ch = Convert.ToChar(j + 65);
                    String index = $"{ch}{i + 1}";
                    workSheet[index].Value = data[i][j];
                    workSheet[index].Style.VerticalAlignment = IronXL.Styles.VerticalAlignment.Center;
                    workSheet[index].Style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Center;

                    if (i == 0)
                    {
                        workSheet[$"{ch}{i + 1}"].Style.Font.Bold = haveHeader;
                    }
                }
            }

            workBook.SaveAs(path);
        }
    }
}
