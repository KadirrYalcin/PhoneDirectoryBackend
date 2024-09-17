using PhoneDirectory.Dtos.PersonDtos;
using PhoneDirectory.Models;
using PhoneDirectory.Services;

namespace PhoneDirectory.Repostory
{
    public class CdnRepostory : CdnService

    {
        public string uploadFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var newname = Guid.NewGuid() + extension;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var location = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, newname);

            var stream = new FileStream(location, FileMode.Create);

            file.CopyTo(stream);

            return "uploads/" + newname;

        }
    }
}
