
using Server.Entities;
using Shared.Models;

namespace Server.Utils;

public static class DTOMapper
{
    public static VideoUrl ToEntity(this VideoUrlDTO obj)
    {
        return new()
        {
            Url = obj.Url,
            Id = obj.Id,
            BlogId = obj.BlogId,
        };
    }

    public static VideoUrlDTO ToDTO(this VideoUrl obj)
    {
        return new()
        {
            Url = obj.Url,
            Id = obj.Id,
            BlogId = obj.BlogId,
        };
    }

    public static Blogimage ToEntity(this BlogImageDTO obj)
    {
        return new()
        {
            ImageId = obj.ImageId.GetValueOrDefault(),
            BlogId = obj.BlogId,
            ImageData = obj.ImageData,
            ContentType = obj.ContentType,
            UploadDate = obj.UploadDate,
        };
    }

    public static BlogImageDTO ToDTO(this Blogimage obj)
    {
        return new()
        {
            ImageId = obj.ImageId,
            BlogId = obj.BlogId.GetValueOrDefault(),
            ImageData = obj.ImageData,
            ContentType = obj.ContentType,
            UploadDate = obj.UploadDate.GetValueOrDefault(),
        };
    }
}
