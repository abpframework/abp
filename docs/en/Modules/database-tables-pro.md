# Database Tables

This documentation describes all database tables and their purposes. You can read this documentation to get general knowledge of the database tables that come from each module.

## [Chat Module](chat.md)

### ChatUsers

This table stores chat users. When you create a new user, a new identity user is created and a new record is added to this table as well.

### ChatConversations

This table stores information about online chat conversations between users. When a user starts a new conversation, a new record is added to this table.

### ChatMessages

This table stores information about Chat messages, including the text, creator id, creation date, and other relevant details. It enables filtering and searching for messages, and tracking metrics such as views.

### ChatUserMessages

This table can store information about the Chat user messages, including the message id, sender, receiver and other relevant information. It can also be used to filter and search messages, as well as to track the metrics associated with the messages, such as views and response time.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [ChatMessages](#chatmessages) | Id | Link to the message. |

## [Cms Kit Module](cms-kit/index.md)

### CmsNewsletterPreferences

This table maintains the user's preferred settings for receiving newsletters through the [CMS Kit Newsletters system](cms-kit/newsletter.md). The information stored in this table helps the CMS system to deliver preferred and specific newsletters to each user, providing a more personalized experience for subscribers.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [CmsNewsletterRecords](#cmsnewsletterrecords) | Id | Link to the newsletter record. |

### CmsNewsletterRecords

This table stores information about users that are registered for the newsletter, such as their email addresses. You can query the email addresses of users registered for the newsletter.

### CmsPolls

This table stores information about the polls created using the [CMS Kit Poll system](cms-kit/poll.md). Polls can be used to gather user feedback or opinions on a topic, and this table stores details such as the poll question and vote count.

### CmsPollOptions

This table stores information about the poll options that are associated with each poll. The CMS kit provides a poll system for creating and managing online polls, and this table helps to keep track of the different options for each poll.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [CmsPolls](#cmspolls) | Id | To match the poll option with the poll. |

### CmsPollUserVotes

This table stores the user votes for the polls that are managed by the CMS Kit Module. It allows to keep track of the users who have voted in a particular poll and their selected options.

### CmsShortenedUrls

This table stores URL mappings in the system and is used by the [URL Forwarding](cms-kit/url-forwarding.md) feature of the CMS Kit Module. The [URL forwarding system](cms-kit/url-forwarding.md) allows the creation of URLs that redirect to other pages or external websites. 

## [File Management Module](file-management.md)

### FmDirectoryDescriptors

This table is utilized by the [File Management system](file-management.md) to manage directories by using the [BlobStoring](https://docs.abp.io/en/abp/latest/Blob-Storing) module.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [FmDirectories](#fmdirectorydescriptors) | Id | Links the directory descriptor with the directory. |

### FmFileDescriptors

This table is used by the [File Management system](file-management.md) to store information about the files and directories in the application, including metadata such as the file name, size, and creation date.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [FmDirectoryDescriptors](#fmdirectorydescriptors) | Id | Links the file descriptor with the directory. |

## [Form Module](forms.md)

### FrmForms

This table stores information related to forms created using the [Forms Module](forms.md).

### FrmQuestions

This table is used to store information about the questions used in the [Forms Module](forms.md), such as the type of the question, the text of the question. This information is used to build and display forms to the user for data collection and analysis.

### FrmChoices

This table stores the choices or options for a form question in the [Forms Module](forms.md). The table is used to store the information needed to display choices in a form question, such as the text and value.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [FrmQuestions](#frmquestions) | Id | Links the choice with the question. |

### FrmFormResponses

This table holds information on the results that the users have responded with to the forms. It can be used to calculate statistics such as how many people have answered a form.

### FrmAnswers

This table stores the answers provided by application users for a certain form. It can be used to calculate statistics, such as how many people have responded to a particular form question.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [FrmFormResponses](#frmformresponses) | Id | Links the answer with the form response. |

## [Gdpr Module](gdpr.md)

### GdprRequests

This table stores requests made by users to access or delete their personal data collected by the application as part of the GDPR compliance.

### GdprInfo

This table holds information related to the personal data that has been collected by modules/applications. This information is used when a user requests her/his personal data. Also, when a user requests to delete their personal data, related records with the user are removed from this table.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [GdprRequests](#gdprrequests) | Id | Links the GDPR information with the GDPR request. |

## [Language Management Module](language-management.md)

### AbpLanguages

This table is important for supporting multiple languages in an application and for providing a better user experience by allowing users to switch between different languages.

### AbpLanguageTexts

This table is important for providing a better user experience by allowing the application to display text in the user's preferred language.

## [Payment Module](payment.md)

### PayPaymentRequests

This table stores information about the payment requests initiated by users.

### PayPaymentRequestProducts

This table keeps track of the products associated with each payment request. You can use this table to collect metrics, such as how many products have been bought in a time interval.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [PayPaymentRequests](#paypaymentrequests) | Id | Links the payment request product with the payment request. |

### PayPlans

This table contains information about the different plans offered within the application for recurring payments through the [Payment Module](payment.md).

### PayGatewayPlans

This table maps the plans offered in the application to the corresponding plans available in the integrated payment gateway to process recurring payments.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [PayPlans](#payplans) | Id | Links the gateway plan with the plan. |

## [SaaS Module](saas.md)

### SaasEditions

This table stores information about the different editions of the application. Each record represents an edition and contains information about the edition, such as the name and other details.

### SaasTenants

This table stores information about the tenants. Each record represents a tenant and contains information about the tenant, such as the name, password, and all other relevant details.

### SaasTenantConnectionStrings

This table stores information about the tenant database connection strings. When you define a connection string for a tenant, a new record will be added to this table. You can query this database to get connection strings by tenants.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [SaasTenants](#saastenants) | Id | Links the connection string with the tenant. |

## [Text Template Management Module](text-template-management.md)

### AbpTextTemplateContents

This table can store reusable text templates that can be easily referenced and rendered by the application at runtime. Each record in the table represents a text template content and allows to manage and track the text template contents effectively.
