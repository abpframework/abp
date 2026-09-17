using System;

namespace Volo.Abp.BlobStoring.Bunny;

[Serializable]
public class BunnyStorageZoneModel
{
    public int Id { get; set; }

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Region { get; set; }

    public bool Deleted { get; set; }
}
