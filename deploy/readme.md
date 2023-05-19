# ABP Framework Release Steps

## 1-) Set your secret keys

The following files are ignored from GitHub, therefore you need to create these files in your local environment.

* create `npm-auth-token.txt` file in this folder and write the npmjs.org auth token. it's a GUID type.
* create `nuget-api-key.txt` file this folder and write your nuget.org API key. 
* create `ssh-password.txt` file and write `jenkins`  user SSH password.

## 2-) Run the commands

The commands are the followings:

- **1-fetch-and-build.ps1**
  - You need to enter the branch name. eg: `rel-5.0`
  - You need to enter the new version for this release. eg: `5.0.0-rc.2`
- **2-nuget-pack.ps1**
- **3-nuget-push.ps1**
- **4-npm-publish.ps1**
- **5-git-commit.ps1**
- **6-new-github-release.ps1**
  - Note: The step 6 is not active. You need to manually make GitHub release from https://github.com/abpframework/abp/releases/new 
    * create tag. eg: `5.0.0-rc.1`
    * create the release from the related branch eg: `rel-5.0`
    * enter the release title. eg: `5.0.0-rc.1`
    * auto generate release notes and delete merge lines
    * check `This is a pre-release` if it's a RC version.
    * click `Publish release` button.
- **7-download-release-zip.ps1**

