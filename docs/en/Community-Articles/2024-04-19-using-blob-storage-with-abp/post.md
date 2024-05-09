# Using Blob Storage with ABP
ABP Framework provides a comprehensive solution to meet the needs of modern application development, while addressing the important requirement of BLOB Storing. ABP Framework [provides an easy integration for Blob Storing](https://docs.abp.io/en/abp/latest/Blob-Storing) and offers many storage services that you can easily integrate. In addition to efficiently storing large files, these services offer significant advantages such as scalability, security and backup.

## What is Blob Storage ?

Blob Storage is a service for storing unstructured data. It is becoming increasingly important to efficiently store and manage large data types (e.g. images, videos, documents). Blob Storage was developed to meet these needs and offers a secure solution with the advantages of scalability, durability and low cost. 

   ![Blob Stroge](./images/blob-storage.png)


## How to use Blob Storage ?

I would like to explain this to you with an example.For example, storing large files such as user profile pictures in the database negatively affects the performance and database.You can store this data using Blob storage. One of the advantages of storing user profile pictures in blob storage is that it improves database performance. Blob storage is a more efficient option than storing large size files in the database and allows database queries to run faster. Furthermore, blob storage provides scalability, so that the number of profile pictures can grow with the number of users, but without storage issues. This approach also maintains database normalization and makes the database design cleaner. 

 How do we store user profile pictures with Blob Storage using ABP Framework? 

- #### Step 1: Configure the Blob Container

Define a Blob Container named `profile-pictures` using the `[BlobContainerName("profile-pictures")]` attribute. 

````csharp
[BlobContainerName("profile-pictures")]
public class ProfilePictureContainer
{
    
}
````
- #### Step 2: Create the ProfileAppService (Saving & Reading BLOBs)

Create the `ProfileAppService` class and derive it from the `ApplicationService` class. This class will perform the necessary operations to store and retrieve profile pictures.

````csharp
using Volo.Abp.Application.Services;

public class ProfileAppService : ApplicationService
{
    // Code snippets will come here
}
````

- #### Step 3: Inject the `IBlobContainer` Service

Inject the `IBlobContainer` service, in the constructor of the `ProfileAppService` class. The `IBlobContainer` is the main interface to store and read BLOB and is used to interact with the container.

````csharp
private readonly IBlobContainer<ProfilePictureContainer> _blobContainer;

public ProfileAppService(IBlobContainer<ProfilePictureContainer> blobContainer)
{
    _blobContainer = blobContainer;
}
````

- #### Step 4: Save Profile Picture

The SaveProfilePictureAsync method is used to store the user's profile picture. A unique name is generated based on the user's credentials and the profile picture byte array with this name is saved in the Blob Container.

````csharp
public async Task SaveProfilePictureAsync(byte[] bytes)
{
    var blobName = CurrentUser.GetId().ToString();
    await _blobContainer.SaveAsync(blobName, bytes);
}
````

- #### Step 5: Getting Profile Picture

The GetProfilePictureAsync method is used to get the user's profile picture. A profile picture byte array is retrieved from the Blob Container with a specified name based on the user's credential.

````csharp
public async Task<byte[]> GetProfilePictureAsync()
{
    var blobName = CurrentUser.GetId().ToString();
    return await _blobContainer.GetAllBytesOrNullAsync(blobName);
}
````


Finally, add controls in the user interface that will allow users to upload and view their profile pictures. These controls will perform the operations by calling the corresponding methods in the ProfileAppService class.

These steps cover the basic steps to store user profile pictures with Blob Storage using the ABP Framework. [Check out the documentation for more information.](https://docs.abp.io/en/abp/latest/Blob-Storing)


## What are the Advantages/Disadvantages of Keeping the BLOBs in a Database? 

#### Advantages:

- Data Integrity and Relational Model: To ensure data integrity and preserve the relational model, it is important to store blob data in the database. This approach preserves the relationships between data and maintains the structural integrity of the database. 

- A Single Storage Location: Storing blob data in the database allows you to collect all data in a single storage location. This simplifies the management of data and increases data integrity. 

- Advanced Security Controls: Database systems often offer advanced security controls. Storing blob data in a database allows you to take advantage of these security features and ensures that data is accessed by authorized users. 

#### Disadvantages: 

- Performance Issues: Storing blob data in a database can negatively impact database performance. Oversized blob data can slow down query processing and reduce database performance. 

- Storage Space Issue: Storing blob data in the database can increase the size of the database and require more storage space. This can increase storage costs and complicate infrastructure requirements. 

- Backup and Recovery Challenges: Storing blob data in a database can make backup and recovery difficult. The large size of blob data can make backup and recovery time-consuming and data recovery difficult. 


## Other Blob Storage Providers

ABP Framework provides developers with a variety of options and flexibility by offering integration infrastructure for multiple cloud providers. This makes it easy for users to choose between different cloud platforms and select the most suitable solution for their business needs. 


- Azure Blob Storage: A cloud storage service offered on the Microsoft Azure platform. It is used to store and access large amounts of data. It supports various data types such as files, images, videos and provides high scalability. ABP Framework provides integration with  [Azure Blob Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Azure). 

-  Aliyun Object Storage Service (OSS): OSS, Alibaba Cloud's cloud storage service, is an ideal solution for use cases such as big data storage, backup and media storage. It offers flexible storage options and provides a high level of security. ABP Framework interfaces with [Aliyun Blob Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Aliyun), making it easier for developers to manage data storage and access.

- MinIO: MinIO is known as an open source object storage system and offers an Amazon S3 compatible cloud storage solution. It is a high-performance, scalable and fast storage service. ABP Framework integrates with [MinIO Blob Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Minio) to provide developers with cloud-based file and object storage.

- Amazon Simple Storage Service (S3): Amazon S3 is a cloud storage service offered on the Amazon Web Services (AWS) platform. It can be used to store virtually unlimited amounts of data. It provides high durability, scalability and low cost.ABP Framework integrates with [Amazon S3 Blob Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Aws) to provide developers with cloud-based file and object storage.
