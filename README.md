# MeteoRV

Application de réalité virtuelle sur le thème de la prévision météorologique.

## ☁️ Concept

Notre application en réalité virtuelle permet de visualiser la météo comme une **fenêtre ouverte sur l’extérieur**.

L’utilisateur fait face à un paysage représentant les conditions météorologiques (ciel dégagé, pluie , neige), tout en conservant des informations contextuelles superposées à la fenêtre.

---

## 🎮 Interactions
L’application utilise le **XR Interaction Toolkit** de Unity.

### 🔹 Navigation temporelle
L’utilisateur peut interagir avec des boutons 3D pour :

* Passer au **jour suivant / précédent**
* Passer à l’**heure suivante / précédente**

Les boutons peuvent être :
* **Actifs** (rouge)
* **Désactivés** (gris) si aucune action possible

Le bouton sur le controller pour intéragir se met en avant lorsqu'on est en hover
### 🔹 Téléportation

L’utilisateur peut se déplacer dans la scène de cette manière :

1. Pointer la zone de téléportation
2. Quand l'utilsateur peut se téléporter, alors le controller projette un trait pour indiquer que la téléportation est possible 
3. Appuyer sur le bouton d’activation (trigger)
4. L’utilisateur est téléporté à la position ciblée et est tourné vers la fenêtre

---

## 🌐 API Météo
Les données météorologiques proviennent de l’API suivante :

```
https://api.open-meteo.com/v1/forecast
```

### 🔹 Paramètres utilisés

* Latitude / Longitude (L'application récupère les informations de Chicoutimi)
* Température horaire (`temperature_2m`)
* Code météo (`weathercode`)
* Plage de dates : le jours même + 6 jours

### 🔹 Structure des données

Les données sont transformées en objets internes :

* `WeatherData`

  * contient une liste de `WeatherDay`

* `WeatherDay`

  * une date
  * une liste de `WeatherHour`

* `WeatherHour`

  * heure
  * température
  * code météo

### 🔹 Exemple d'utilisation

Accès à une heure précise :
```csharp
weatherData.weatherDays[2].weatherHours[15];
```

👉 Correspond à :
* Jour +2
* Heure 15h

---

## 🧱 Architecture

Le projet est structuré autour de plusieurs composants principaux :

### 🔹 WeatherManager

* Récupère les données depuis l’API
* Transforme les données en objets exploitables
* Notifie les autres systèmes lorsque les données sont prêtes

### 🔹 WeatherController

* Gère la navigation (jour / heure)
* Met à jour l’état courant
* Envoie les données pour les afficher

### 🔹 WeatherDisplay

* Met à jour l’interface utilisateur
* Affiche :
  * date
  * heure
  * température
  * météo
  * erreur (dans le cas où l'api n'a pas donnée de réponse correcte)

### 🔹 InteractionButtons

* Gère les intéractions des boutons
* Active / désactive selon le contexte
* Gère le feedback visuel (actif, désactivé)

---

## 🎨 Assets (Unity Asset Store)
Le projet utilise des assets provenant du Unity Asset Store pour :

* ...

---

## ⚙️ Environnement de développement

> Unity Editor version : **6000.3.5f1**

---
