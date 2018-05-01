# Description

Cette API développée en C# vous permet d'accéder aux opérations disponibles dans la console d'administration Krios (https://console.komodo.ch/krios).
Ce dépôt contient les librairies de cette API ainsi qu'un code d'exemple qui approvisionne des 
sessions SwissDesk X.

# Structure générale

Un **TenantKrios** est une entité globale représentant une personne morale ou phyisque. D'un point de vue administratif, il peut signer un ou plusieur accords (*agreements*) ce qui lui donne la possiblité d'utiliser différentes resources chez Krios.

D'un point de vue technique, le **TenantKrios** administre ses différentes ressources au travers de **Pool**. Ces derniers sont des conteneurs sur lesquels des droits d'accès peuvent être appliqué. 

Un **utilisateur API** est simplement un utilisateur d'un **TenantKrios** spécifique. Si ce dernier dispose des droits nécessaires, il peut créer d'autres **TenantKrios**, en être le revendeur, avoir des accès sur ses **Pool** de ressources etc.

# Utilisation

l'API dispose de classes pour gérer la partie administrative dans le namespace ```General``` et du namespace ```Resource``` pour administrer les ressources.

Comme la création des ressources est asynchrone, l'API met à disposition une classe ```EventChannel``` permettant d'attendre sur les opérations faites sur ces ressources, d'enregistrer des callback ainsi que de réagir à d'éventuelles erreurs.
Les callbacks sont appelés avec un ```Scope``` en paramètre. Ce scope permet de récupérer les objets de base de l'API et permet aussi d'ajouter/supprimer d'autres variables utiles.

# Exemple

L'exemple fournit crée un **TenantKrios** dans lequel il provisionne une session *SwissDesk X* de type Crésus Cloud (CCloud). L'enchainement des callbacks permet de créer les différentes ressources de manières séquentielles.
L'Ordre de création peut être parallélisé ou modifié suivant les prérequis de chaque ressource.

# Ressources disponibles

## UserADKomodo

Représente un utilisateur de l'ActiveDirectory Komodo. Les champs obligatoires sont ```R_Name``` (nom d'utilisateur), ```P_DisplayName``` et ```P_Pwd```.

### Dépendance

Aucune.

## SDXProfile

Représente le profil d'un **UserADKomodo**. Cet objet symbolise tous les datas et paramètres de l'utilisateur *SwissDesk X*

### Dépendance

Dépend d'un **UserADKomodo** et d'un **Storage** de type **ProdType**

## SDXSession

Représente la session PCoIP d'un **SDXProfile**. Ces sessions *SwissDesk X* sont crées à l'aide de template appelé modConfig. Chaque template dispose d'une liste de services pouvant être activé sur la session.

### Dépendance

Dépend d'un **SDXProfile** et d'un **UserADKomodo**

## SDXService

Représente les services d'une session **SDXSession**. Un service peut être un programme comme Office 365 ou Crésus Salaire par exemple. Chaque service peut être instancié avec une configuration particulière, transmise dans un JSON.

### Dépendance

Dépend d'un **SDXSession**, d'un **SDXProfile** et d'un **UserADKomodo**

## Storage

Représente un espace de stockage. Il est d'un type spécifique, définit par les **StorageType**.

### Dépendance

Aucune.

## StorageHeadFolder

Représente un dossier à la racine du stockage parent.

### Dépendance

Dépend d'un **Storage**
