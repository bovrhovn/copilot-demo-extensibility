# Sample copilot extensibility options

This is a sample project that demonstrates how to use the Copilot extensibility options to customize the behavior of the Copilot plugin. More on Microsoft Docs site [here](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/). 

![Anatomy of M365 Copilot](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/assets/images/anatomy-m365-copilot.png)

As a developer, you can extend Microsoft 365 Copilot to build Copilot agents to bring custom knowledge, skills, and process automation into Microsoft 365 Copilot, tailoring it to suit the unique needs of your customers.

Copilot agents are fundamentally composed of Custom Knowledge (via instructions and grounding data), Custom Skills (including Actions, Triggers, and Workflows), and Autonomy (including planning, learning, and escalation).

![Copilot Agents](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/assets/images/anatomy-agents.png)

## Prerequisites

1. An active [Azure](https://www.azure.com) subscription - [MSDN](https://my.visualstudio.com) or trial
   or [Azure Pass](https://microsoftazurepass.com) is fine - you can also do all of the work
   in [Azure Shell](https://shell.azure.com) (all tools installed) and by
   using [Github Codespaces](https://docs.github.com/en/codespaces/developing-in-codespaces/creating-a-codespace)
2. [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/) installed to work with Azure
3. [GitHub](https://github.com/) account (sign-in or join [here](https://github.com/join)) - how to authenticate with
   GitHub
   available [here](https://docs.github.com/en/get-started/quickstart/set-up-git#authenticating-with-github-from-git)
4. [RECOMMENDATION] [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed - we do recommend an editor like [Visual Studio Code](https://code.visualstudio.com) to be able to write
   scripts, YAML pipelines and connect to repos to submit changes.
5. [OPTIONAL] GitHub CLI installed to work with GitHub - [how to install](https://cli.github.com/manual/installation)
6. [OPTIONAL] [Github GUI App](https://desktop.github.com/) for managing changes and work
   on [forked](https://docs.github.com/en/get-started/quickstart/fork-a-repo) repo
7. [OPTIONAL] [Windows Terminal](https://learn.microsoft.com/en-us/windows/terminal/install)

If you will be working on your local machines, you will need to have:

1. [Powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed
2. git installed - instructions step by step [here](https://docs.github.com/en/get-started/quickstart/set-up-git)
3. [.NET](https://dot.net) installed to run the application if you want to run it
4. an editor (besides notepad) to see and work with code, yaml, scripts and more (for
   example [Visual Studio Code](https://code.visualstudio.com))

## Scripts

Scripts are available in [scripts folder](./scripts). The scripts are written
in [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/overview?view=powershell-7.2).

1. [Add-DirToSystemEnv.ps1](./scripts/Add-DirToSystemEnv.ps1) - adds a directory to the system environment variable
   PATH
2. [Install-AZCLI.ps1](./scripts/Install-AZCLI.ps1) - installs Azure CLI
3. [Install-Bicep.ps1](./scripts/Install-Bicep.ps1) - installs Bicep language

## Scripts

Scripts to help deploying to the cloud and working with demos are available in the `scripts` directory. They are written
in PowerShell and use Bicep or Azure CLI do deploy infrastructure as code and to help with applications.

[Docker files](./containers) are available to build and run the application in containers. You can also leverage helper
script [Compile-Containers.ps1](./scripts/Compile-Containers.ps1) to build containers
using [Azure Container Registry task builders](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-tutorial-build-task).

# Links and additional information

1. [Copilot Extensibility](https://learn.microsoft.com/en-us/microsoft-365-copilot/extensibility/)
2. [Azure Portal](https://portal.azure.com/)
3. [Azure Container Apps](https://azure.microsoft.com/en-us/services/container-apps/)
4. [Azure Kubernetes Services](https://azure.microsoft.com/en-us/services/kubernetes-service/)

