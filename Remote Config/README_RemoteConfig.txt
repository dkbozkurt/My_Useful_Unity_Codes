// Dogukan Kaan Bozkurt
//		github.com/dkbozkurt

README - Remote Config
Ref video : https://www.youtube.com/watch?v=14njHI4CGgA&ab_channel=Brackeys

Without pushing an update and without even opening Unity,
"REMOTE CONFIG" allows you to make changes after it has already launched.

 - PRE STEPS -
 
1) Install "Remote Config" through the package manager.
Window > Package Manager > Search: Remote Config > Install

2) Open up the remote config window.
Window > Remote Config

3) In order to use Remote Config, we need to link our project to a Unity cloud project ID
Window > General > Services (Dont forget to Sign-in from "Account")

4) If there is no Project ID generate one.
Services > General settings > Select an organization from a enum bar > Generate Project ID

When its done, our "Remote Config" window should be ready to use.


 - MID STEPS -
 
1) First make sure we complete the synced by clicking "Pull" to get all changes from the cloud.

2) Select the environment, 

	The release environment is for finished builds,
	while the development/ production is used while testing in the Editor.

(Development environment can be used for test builds by 
File > Build Setting > Development Build (Checkbox = true) before exporting)

3) Adding Remote Key

Remote Config > Add Setting, and change the settings of the key from the window.

4) Save added Remote Keys

Press "Push" button after changes on keys to save them on remote cloud.

5) Changing remotes on browser

Remote Config > View in Dashboard > Log-in > Open navigation > LiveOps > Remote Config > Environments >
Select the environment's "View Config Values" and you will see the config values.

 - IN GAME (Script) STEPS -

1) Add "using Unity.RemoteConfig;" namespace

2) Create 2 struct variables.

3) ... In scripts.

 - ADDITIONAL(RULES) STEPS -

	Rules allow us to target on this parts of out audience.
	
1) Add a key, to referance in specific season or time interval. To Add rules: 

Remote Config > View in Dashboard > LiveOps > Remote Config > Environments > View Config Values > View Overrides > Create Campaign
