namespace MeetMind.Infrastructure.Storage;

public class StorageOptions
{
    public const string SectionName = "Storage";

    public string Provider { get; set; } = "LocalDisk";
    public string LocalDiskPath { get; set; } = "uploads";
    public string? AzureBlobConnectionString { get; set; }
    public string? AzureBlobContainerName { get; set; }
    public string? AwsAccessKey { get; set; }
    public string? AwsSecretKey { get; set; }
    public string? AwsBucketName { get; set; }
    public string? AwsRegion { get; set; }
}
