# How to Contribute to ABP Framework

## What is Open Source?

Open source software is code designed to be publicly available. Anyone can view, use, modify and distribute your project and code. The fact that the code is open source makes it a natural community and open for improvement. This enables ideas and thoughts to spread rapidly.

## What is ABP Framework?

ABP Framework is a complete infrastructure to create modern web applications by following the software development best practices and conventions. ABP Framework is completely free, open source and community-driven and provides a free theme and some pre-built modules. ABP is a modular framework and the Application Modules provide pre-built application functionalities.

## Step 1: Fork the Project

The first thing we need to do now is to fork the open source project. Forking will create a copy of the project in your own github account. This will allow users to make changes to the code without affecting the original repository. Just press the fork key in the project.

![fork-image](images/fork-project-image.png)

After forking, it will create a new repo in your own github profile.

![fork-image-profile](images/fork-project-profile.png)

## Step 2: Clone the Project

In order to develop on the project, you need to clone it to your local. After clicking on the code button, select your preferred cloning method and copy the link. 

![clone-image](images/clone-image.png)

You can run the copied link as on your local machine with the `git clone` command.

`git clone <link to repo> `

If you want you can clone it using Github Desktop and do it in a simpler way, but in this article we will use commands.

## Step 3: Create a New Branch

In this step, you need to create a new branch of your own before you start developing it. Open the main folder of the repo you cloned via a command prompt and use `git checkout -b` to create a new branch. 

`git checkout -b new-branch`

![branch-image](images/branch-image.png)

## Step 4: Development

Choose a suitable IDE to develop on the new branch you created. In order not to complicate things, we will create a `Developers.md` file and process it. Let's enter a sample text in the Developers file.

![developer-list](images/developer-list.png)

## Step 5: Commit

You need to use the `git add .` command to add the file you modified. This command adds files from the working directory to the staging area for git. Then use `git commit -m` to commit the changes permanently.

`git add .`

`git commit -m "<your commit message>"`

![commit-image](images/commit-image.png)

## Step 6: Push the Changes

The changes you have made so far are only visible on your local machine. The `git push` command is used to push these changes to the repository you forked.

`git push origin <your branch name> `

![push-image](images/git-push-image.png)

## Step 7: Create a Pull Request

After the push, go to the github repo you forked. The `Compare and pull request` button will appear on the main page of the repository. Press the button for pull request

![compare-pull-request](images/pull-request-image.png)

This will send a pull request to the original repository. Finally, click on the `Create pull request` button. If the pull request is approved and merged, your changes will also appear in the main repository.

![open-pull-request](images/open-pull-request-image.png)

That's it! You have contributed to an open source project with your development.



