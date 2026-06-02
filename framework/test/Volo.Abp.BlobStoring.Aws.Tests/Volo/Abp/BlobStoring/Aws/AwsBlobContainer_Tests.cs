namespace Volo.Abp.BlobStoring.Aws;

/*
 To run this integration test, populate user secrets (id `9f0d2c00-80c1-435b-bfab-2c39c8249091`)
 with credentials for real AWS S3 or an S3-compatible service (Cloudflare R2, MinIO, etc.):

   {
     "Aws:AccessKeyId": "...",
     "Aws:SecretAccessKey": "...",
     "Aws:Region": "us-east-1",                    // "auto" for Cloudflare R2
     "Aws:ServiceURL": "",                         // e.g. https://<account>.r2.cloudflarestorage.com for R2, http://localhost:9000 for MinIO
     "Aws:DisablePayloadSigning": "false",         // set "true" for R2 (requires HTTPS); leave "false" for MinIO and AWS S3
     "Aws:ContainerName": "",                      // leave empty to let the test create a unique bucket; set to a pre-existing bucket for services that disallow bucket creation
     "Aws:CreateContainerIfNotExists": "true"      // set "false" when using an existing bucket
   }

 When `ContainerName` is supplied, the test prefixes every object with `abp-aws-test-run-{guid}/`
 so cleanup deletes only what this run created (the bucket itself is left untouched).

 Then uncomment the class below:

public class AwsBlobContainer_Tests : BlobContainer_Tests<AbpBlobStoringAwsTestModule>
{
}
*/
