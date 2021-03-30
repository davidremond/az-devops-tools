# Az DevOps Tools

**az-devops-tools** provides additional features for Azure DevOps Services.

The tool provides these features :
- Checking status of templates and pipelines and output a migration path for pipelines and templates life cycle.

New features will be available soon.

## Installation
The tool is available on [Nuget](https://www.nuget.org/packages/az-devops-tools).

You can install the tool by executing this command :

```bash
$ dotnet tool install az-devops-tools --global
```

## Help

Use this command to list all available commands.

```bash
$ az-devops-tools --help
```

## Authentication

The tool only use PAT authentication to Azure DevOps. 
Before using the tool, you have to generate a new token with these permissions :
- Code : Read

## Usages

For the moment, the tool provides only one command that check your templates status.

You have to store your templates with the following hierarchy (template name then version) in a Git repository and a folder structure : **\*/#template_name#/#version#/\*\*/*.yml**

For example, you can use the following structure can be used to store your templates :
- jobs
  - deploy_database
    - v1
    - v2
    - v3
  - deploy_webapp
    - v1
    - v2

Templates are referenced like this in your pipelines : **jobs/#template_name#/#version#/template.yml@#repository_name#**

The tool will retrieve all references and identify actions to keep templates repository healthy :
- remove obsolete templates (prevents usage of old versions in your pipelines)
- identify broken references (prevents from pipeline errors)
- identify old templates versions used in pipelines

```bash
$ az-devops-tools templates status --collection-url "https://dev.azure.com/{your organization}" --project-name "{your team project}" --personal-access-token "{your personal access token}" --repository-name "{name of the repository containing templates}"
```

This command provides this output :

```
                     _                                 _              _
   __ _ ____      __| | _____   _____  _ __  ___      | |_ ___   ___ | |___
  / _` |_  /____ / _` |/ _ \ \ / / _ \| '_ \/ __|_____| __/ _ \ / _ \| / __|
 | (_| |/ /_____| (_| |  __/\ V / (_) | |_) \__ \_____| || (_) | (_) | \__ \
  \__,_/___|     \__,_|\___| \_/ \___/| .__/|___/      \__\___/ \___/|_|___/
                                      |_|


********************
* Pipelines status *
********************

Pipeline : 'pipeline1.yml'
    Template 'deploy_webapp' : Up to date !

Pipeline : 'pipeline2.yml'
    Template 'deploy_webapp' : Upgrade from v2 to v4
    Template 'deploy_database' : Up to date !

********************
* Templates status *
********************

Template : 'deploy_webapp'
    Version 'v1' : Unused. Can be removed !
    Version 'v2' : Used by pipelines
    Version 'v3' : Used by pipelines
    Version 'v4' : Used by pipelines

Template : 'deploy_database'
    Version 'v1' : Used by pipelines
```
