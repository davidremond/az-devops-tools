# Az DevOps Tools

**az-devops-tools** fourni des fonctionnalit�s compl�mentaires aux op�rations DevOps.

## Installation

L'outil peut �tre install� en ex�cutant la commande suivante :

```bash
$ dotnet tool install az-devops-tools --global
```

## Utilisation

L'outil ne pr�sente (pour l'instant) qu'une seule commande :

```bash
$ az-devops-tools templates status --collection-url "" --project-name "" --personal-access-token "" --repository-name ""
```

Le r�sultat produit est le suivant :

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

## Aide

```bash
$ az-devops-tools templates status --help
```
