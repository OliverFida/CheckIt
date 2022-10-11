# Check-it - Wartung
## Allgemeines
_Check-it_ besteht aus zwei Komponenten: `checkit_backend` und `checkit_frontend`, welche jeweils das Front- und Backend bereitstellen.
`checkit_backend` basiert auf .NET 6.0 mit ASP.NET Core. `checkit_frontend` auf Clientseitiger HTML-JS-Basis.
## Installation
### Image
Bereitgestellt wird ein VM-Image mit Ubuntu Server 22.04 LTS. Darin laufen als Docker-Services das Backend und Frontend (mittels Apache).
Hostname ist `checkit`.
##### Zugangsdaten
Auf dem Image ist SSH aktiviert. Der Standardbenutzer ist `checkit` mit Passwort `checkit`.
##### Wichtige Pfade
* `/home/checkit/checkit/`
	* `backend/`
		* `checkit.db` - Gespeicherte Daten von Check-it.
		* `docker-compose.yaml` - Docker-Compose-File, hier können Port und DB-Pfade des Backends konfiguriert werden.
	* `frontend/` 
		* `conf/` - Konfigverzeichnis von Apache.
		* `docker-compose.yaml` - Docker-Compose-File, hier können Port und Pfade des Frontends konfiguriert werden.

##### Ports
| Service          | Port |
|------------------|------|
| checkit_frontend | 80   |
| checkit_backend  | 5000 |
|                  |      |
Sowohl der Port für `checkit_frontend` und `checkit_backend` muss von Clients aus erreichbar sein.
## Update
### checkit_frontend
Updates werden als Archiv bereitgestellt.
1. Archiv in `/home/checkit/checkit/frontend/website` entpacken.
### checkit_backend
Updates werden als Archiv bereitgestellt.
1. Archiv entpacken.
2. install.sh ausführen.
3. Den Dienst neustarten (in `/home/checkit/checkit/backend/` den Befehl `docker-compose down && docker-compose up -d` ausführen).
## Konfiguration
### checkit_frontend
#### Port
1. Datei `/home/checkit/checkit/frontend/docker-compose.yaml` öffnen
2. Unter `services` - `checkit_backend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:8080` stellt den Dienst auf Port 1234 bereit.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.
### checkit_backend
#### Port
1. Datei `/home/checkit/checkit/backend/docker-compose.yaml` öffnen.
2. Unter `services` - `checkit_backend` - `ports` die erste Zahl auf den Wunschport ändern. Z.B. `1234:5000` stellt den Dienst auf Port 1234 bereit.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.
#### Datenbankpfad
1. Datei `/home/checkit/checkit/backend/docker-compose.yaml` öffnen
2. Unter `services` - `checkit_backend` - `volumes` den Pfad vor `:/data` auf den Wunschpfad ändern. Z.B. `~/ordner:/data` lässt den Dienst die Datenbank unter `~/ordner/`speichern.
3. Mittels `docker-compose down && docker-compose up -d` den Dienst neustarten.
## Sonstiges
