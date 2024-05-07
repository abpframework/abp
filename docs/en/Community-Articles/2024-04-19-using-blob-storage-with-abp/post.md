# Using Blob Storage with ABP

 ABP Framework is a comprehensive development framework for modern web applications and microservice architectures. This framework offers its users the possibility to quickly build scalable and modular applications. An advantage of this framework is the flexibility to integrate different cloud services or infrastructure components. 

## What is Blob Storage ?

   Blob Storage is a cloud storage service for storing large amounts of data. This service is ideal for storing files, images or other types of data at high scale. The advantages of Blob Storage include scalability, high performance, durability and low cost. In addition, Blob Storage's APIs make it easy to access and manage data. The Blob Storage integration with ABP Framework offers a powerful combination to effectively meet the big data storage needs of your applications. This integration can help you optimize storage costs while improving the performance of your application.

   ![Blob Stroge](./images/blob-storage.png)


## How to use Blob Storage ?

   Nowadays, storing large files such as users' profile pictures has become an important requirement in web applications. Storing such data in the database can negatively impact performance and increase the database size. To solve this problem, Blob storage services can be used. ABP Framework provides a powerful solution to handle such scenarios. So, how do we store user profile pictures with Blob Storage using ABP Framework?

- #### Step 1: Blob Container Identification

The first step is to define a Blob Container for storing profile pictures. The Blob Container represents the storage space that will be used to store the profile pictures.

   ![Step 1](./images/step1.png)

- #### Step 2: Create ProfileAppService

Create the ProfileAppService class and derive it from the ApplicationService class. This class will perform the necessary operations to store and retrieve profile pictures.

   ![Step 2](./images/step2.png)

- #### Step 3: Blob Container Connection

In the constructor method, inject the dependency into the IBlobContainer interface. This interface is used to interact with the Blob Container.

   ![Step 3](./images/step3.png)

- #### Step 4: Save Profile Picture

The SaveProfilePictureAsync method is used to store the user's profile picture. A unique name is generated based on the user's credentials and the profile picture byte array with this name is saved in the Blob Container.

   ![Step 4](./images/step4.png)

- #### Step 5: Getting Profile Picture

The GetProfilePictureAsync method is used to get the user's profile picture. A profile picture byte array is retrieved from the Blob Container with a specified name based on the user's credential.

   ![Step 5](./images/step5.png)


Finally, add controls in the user interface that will allow users to upload and view their profile pictures. These controls will perform the operations by calling the corresponding methods in the ProfileAppService class.

These steps cover the basic steps to store user profile pictures with Blob Storage using the ABP Framework. [Check out the documentation for more information.](https://docs.abp.io/en/abp/latest/Blob-Storing)


## What are the advantages/disadvantages of keeping in Blob Storage database? 

#### Advantages:

- Data Integrity and Relational Model: To ensure data integrity and preserve the relational model, it is important to store blob data in the database. This approach preserves the relationships between data and maintains the structural integrity of the database. 

- A Single Storage Location: Storing blob data in the database allows you to collect all data in a single storage location. This simplifies the management of data and increases data integrity. 

- Advanced Security Controls: Database systems often offer advanced security controls. Storing blob data in a database allows you to take advantage of these security features and ensures that data is accessed by authorized users. 

#### Disadvantages: 

- Performance Issues: Storing blob data in a database can negatively impact database performance. Oversized blob data can slow down query processing and reduce database performance. 

- Storage Space Issue: Storing blob data in the database can increase the size of the database and require more storage space. This can increase storage costs and complicate infrastructure requirements. 

- Backup and Recovery Challenges: Storing blob data in a database can make backup and recovery difficult. The large size of blob data can make backup and recovery time-consuming and data recovery difficult. 


## Information about Blob Storage entagrations  

- Azure Blob Storage: A cloud storage service offered on the Microsoft Azure platform. It is used to store and access large amounts of data. It supports various data types such as files, images, videos and provides high scalability. To learn more about [Azure Blob Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Azure), visit the Azure Blob Storage page. 

- Aliyun Object Storage Service (OSS): OSS, Alibaba Cloud's cloud storage service, is an ideal solution for use cases such as big data storage, backup, and media storage. It offers flexible storage options and provides a high level of security. For more information, please visit [Aliyun Object Storage](https://docs.abp.io/en/abp/latest/Blob-Storing-Aliyun) Service (OSS) page.

- MinIO: MinIO is known as an open source object storage system and offers an Amazon S3 compatible cloud storage solution. It is a high-performance, scalable and fast storage service. It can be used especially in private cloud or hybrid cloud environments. For more information, you can visit [MinIO's Official](https://docs.abp.io/en/abp/latest/Blob-Storing-Minio) Website. 

- Amazon Simple Storage Service (S3): Amazon S3 is a cloud storage service offered on the Amazon Web Services (AWS) platform. It can be used to store virtually nlimited amounts of data. It provides high durability, scalability and low cost. For more information, you can visit the [Amazon S3 Documentation page](https://docs.abp.io/en/abp/latest/Blob-Storing-Aws). 
