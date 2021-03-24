# Az DevOps Tools

**az-devops-tools** provides additional features for Azure DevOps Services.

The tool provides these features :
- Checking status of templates and pipelines and output a migration path for pipelines and templates life cycle.

New features will be available soon.

## Installation

You can install the tool by executing this command :

```bash
$ dotnet tool install az-devops-tools --global
```

The tool is also available on [Nuget]().

## Help

Use this command to list all available commands.

```bash
$ az-devops-tools --help
```

## Authentication

The tool use only PAT authentication to Azure DevOps. 
Before using the tool, you have to generate a new token with these permissions :
- Code : Read

## Usages

The tool provides (for the moment) only one command that check you templates status :

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
