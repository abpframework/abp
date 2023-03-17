
# On-Prem to Azure: Migration of abp.io Platform to Azure


![abpio-azure](images/abpio-azure.png)

Migrating a Kubernetes platform with a database from our own dedicated servers to Azure can be a compelling task, but it can be a necessary one to take advantage of the benefits that the cloud service offers. In this post, we will discuss the reasons for migrating from a on-premise platform to Azure, the steps taken to create and configure the [abp.io platform](https://abp.io) on Azure, and the benefits gained from the migration.

### On-Premise Server: The old platform

There were several reasons why the decision was taken to migrate from the old on-premise platform to Azure. The first reason was that the Kubernetes cluster and the database were on the same Windows server. This configuration was not ideal as it posed a high risk of security and accessibility issues. Additionally, the Linux virtual machines in Kubernetes were on Windows server and had limited resources. The platform was not in a private network, which further increased the risk of security breaches. Not to mention how challenging the Kubernetes maintenance was and that made another reason for the migration.

### Reasons for Moving to Azure: The new platform

The decision to move to the cloud was made to eliminate the disadvantages of the old platform. The migration to Azure meant that the resources would be independent of each other but faster in terms of communicating with each other. The platform would also take advantage of cloud security and availability. The managed Kubernetes service that Azure offers comes with autoscaling and loadbalancing, which makes the platform more reliable. Finally, the migration to Azure would provide a better quality service to global customers and the community.

### Creating and Configuring the abp.io Platform on Azure

Several steps were taken to create and configure the abp.io platform on Azure. [Terraform](https://www.terraform.io/) was used to create the infrastructure (VM, Private Network, AKS, Postgresql Flexible Server...), while [Ansible](https://www.ansible.com/) was used to configure the Azure resources like Terraform, Helm, Kubectl, Docker, VPN, Redis, Prometheus, Grafana, ElasticSearch, Kibana and so on.... Azure DevOps pipelines and release were used for AKS deployment. The most time-consuming part in this process was to prepare, test and optimize the terraform and ansible settings files.

![terraform-azure](images/terraform-azure.png)

The most important step was to transfer the data in both the database and Kubernetes of the volumes. rsync (remote sync commands) were used to transfer the data from Kubernetes volumes to Azure NFS through the VPN machine. Additionally, `pg_dump` and `pg_restore` were used to transfer the PostgreSQL database through the machine with VPN. 

Before the production environment, the data migration was tested many times for the staging environment. We estimated this migration to take max 1.5 hours. The ABP community was informed that there may be interruptions during the hours designated for the transition to Azure. To inform our customers and community, we created a status page before this migration. The new status page is [status.abp.io](https://status.abp.io). From now on we wil make all the infrastructural announcements on [status.abp.io](https://status.abp.io). We used [Upptime](https://upptime.js.org) which is an open-source uptime monitor and status page provider. During the migration of the production environment, the websites and databases were still up and running. After the data transfer, the only remaining step was to direct the traffic of the already standing abp.io sites to the Azure Kubernetes service via Cloudflare. 

We would like to happily state that **we were offline for only 4 minutes** during this transition.

![az-infra](images/az-infra.png)


### Benefits of Moving to Azure

The migration to Azure resulted in several benefits. The platform is now more reliable, scalable, secure, solid with built-in one-click backup and recovery capabilities for abp.io. Additionally, the critical resources are in a private network, making them more secure than the old environment. There were significant improvements in connection speeds after the migration, which resulted in speeds that were twice as fast as before.

In conclusion, migrating a Kubernetes platform with a database from on-premise to Azure is a complex process that requires careful planning and execution. However, the benefits gained from the migration make the process worthwhile. By moving to Azure, the abp.io platform now has a more reliable and available infrastructure that is more secure than the old environment. The migration also resulted in significant improvements in connection speeds, which ultimately provides a better service to global customers and the community.



