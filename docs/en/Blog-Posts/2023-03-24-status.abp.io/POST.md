# Creating a Custom Status Page for abp.io with Upptime

### Introduction

In today's digital world, providing reliable and transparent information about your platform's availability is essential to maintain trust among your community and customers. With the growing number of abp.io users, we needed a dedicated status page to keep everyone informed about our platform's health. We've already had an internal alert system with health checks for our abp.io websites, but we needed a public-facing status page [status.abp.io](https://status.abp.io/) to share this information with our community and customers, especially during important events like migrating the abp.io platform from on-premise to Azure. To achieve this, we utilized the open-source project [Upptime](https://upptime.js.org/) and built a custom status page on [GitHub Pages](https://pages.github.com/). In this article, we'll guide you through the process of creating our own status page and customizing it to suit our needs.


### Choosing Upptime as our monitoring tool
[Upptime](https://github.com/upptime/upptime) is an open-source, easy-to-use, and cost-effective solution for monitoring websites and APIs. It offers essential features, such as downtime alerts, response time monitoring, and status history. We decided to use Upptime because of its compatibility with GitHub Pages, ease of customization, comprehensive documentation (https://upptime.js.org/docs/) and discord notifications.

Advantages of Upptime
Open-source: Allows easy customization and community support.
GitHub Pages compatibility: Seamless integration with GitHub Pages for hosting.
Cost-effective: Utilizes GitHub Actions, which provides free monitoring within the GitHub Actions usage limits.
Comprehensive documentation: Easy-to-follow instructions for setting up and customizing the status page.
2. Setting up the status page on GitHub Pages
To get started with our custom status page, we followed the instructions in the Upptime documentation (https://upptime.js.org/docs/). Here's a summary of the steps we took:

a. Fork the Upptime template repository
First, we forked the Upptime template repository (https://github.com/upptime/upptime) to our own GitHub account.

b. Configure the GitHub Actions workflow
We then configured our GitHub Actions workflow by editing the .github/workflows files. This included setting up the monitoring frequency, notification settings, and adding our API keys for third-party services if needed.

c. Add the monitored endpoints
We added the monitored endpoints (our abp.io websites) to the repositories.json file. This file is located in the root of the repository and contains a list of URLs that are monitored by Upptime.

json
Copy code
[
  {
    "name": "Website 1",
    "url": "https://website1.abp.io"
  },
  {
    "name": "Website 2",
    "url": "https://website2.abp.io"
  }
]
d. Enable GitHub Pages
Finally, we enabled GitHub Pages for our forked repository by going to the repository's settings and selecting the gh-pages branch as the source. This made our status page accessible at https://status.abp.io/.

3. Customizing the status page
After setting up the default Upptime status page, we focused on customizing it to align with our brand and provide a consistent experience for our community and customers. We made the following changes:

a. Updating the logo and favicon
We replaced the default logo and favicon with our own abp.io branded assets. This involved adding the new image files to the repository and updating the references in the _config.yml file:

yaml
Copy code
logo: "/assets/img/logo.svg"
favicon: "/assets/img/favicon.ico"
b. Customizing the color scheme and typography
We customized the color scheme and typography to match our corporate identity by editing the assets/css/style.scss file:

scss
Copy code
$primary-color: #your-brand-color;
$secondary-color: #your-secondary-color;
$font-family: "Your Font", sans-serif
