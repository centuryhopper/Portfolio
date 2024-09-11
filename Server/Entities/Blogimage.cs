using System;
using System.Collections.Generic;

namespace Server.Entities;

public partial class Blogimage
{
    public int ImageId { get; set; }

    public int? BlogId { get; set; }

    public byte[]? ImageData { get; set; }

    public string? ContentType { get; set; }

    public DateTime? UploadDate { get; set; }

    public virtual Blog? Blog { get; set; }
}
