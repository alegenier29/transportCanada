# TransportCanada

PROCÉDURE D'INSTALLATION
Installation du Site web et de l'API 3 pour Windows IIS:
1. Installer ASP.NET Core Runtime 3.1.301 - Windows Hosting Bundle (https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.5-windows-hosting-bundle-installer)
2. Exécuter un IISRESET
3. Créer un répertoir ou le site sera hébergé
4. Copier le contenu de \Release\netcoreapp3.1\publish au dossier créé au point 3
5. Ouvrir IIS
6. Créer un Application Pool nommé NetCore avec un ".NET CLR version" de "No Managed Code"
7. Changer les permissions du dossier créé au point 3 ajouter "IIS AppPool\NetCore" avec la permission Modify
8. Créer un nouveau site ou ajouter un application à un site existant, donner un nom, pointer au dossier créé au point 3, puis choisir l'Application Pool créé au point 6
9. Naviguer au nouveau site

# Hébergement
Hébergement https://api320200710160911.azurewebsites.net/tc


# Logiciels et versions requis
ASP.NET Core Runtime 3.1.301 - Windows Hosting Bundle (https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.5-windows-hosting-bundle-installer)


# Instruction pour reconstruire les APIs
L'API 3 peut être reconstruit en ouvrant la Solution Recall.sln. Ensuite, faire un clique droit sur le projet API3 et cliquer sur "Publish". S'il n'y a pas déjà un profile de créé, créer un profile "File System" (prendre note du dossier de destination) et garder les paramètres par défault. Cliquer finalement sur "Publish" et copier les fichiers résultant au site IIS.


# Autres informations
Le site web devrait populer le lien pour l'API 3 automatiquement, si non le lien à utiliser est le lien de base du site (ex: https://api320200710160911.azurewebsites.net/tc/) suivi de API3/.

# Operations exposés

POST: https://api320200710160911.azurewebsites.net/tc/API3. Envoie un tableau de rappels.
GET: https://api320200710160911.azurewebsites.net/tc/API3/SystemType/Freins. L'url precedant obtient tous les rappels où le type de système concerné est le freins.

Pour tous les APIs si le champs de recherche contient du texte l'option Visionner et Télécharger seront filtré d'après la valeur dans la boite de recherche.
