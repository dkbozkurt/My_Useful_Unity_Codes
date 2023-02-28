using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

namespace PolygonArsenal
{

public class PolygonSceneSelect : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	public bool GUIHide4 = false;
	public bool GUIHide5 = false;
	
	//Combat Scenes
	
    public void CBLoadSceneMissiles()		{ SceneManager.LoadScene("PolyMissiles");	}
	public void CBLoadSceneBeams()			{ SceneManager.LoadScene("PolyBeams"); 		}
	public void CBLoadSceneBeams2()			{ SceneManager.LoadScene("PolyBeams2"); 	}
	public void CBLoadSceneAura()			{ SceneManager.LoadScene("PolyAura"); 		}
	public void CBLoadSceneAura2()			{ SceneManager.LoadScene("PolyAura2");	 	}
	public void CBLoadSceneAura3()			{ SceneManager.LoadScene("PolyAura3"); 		}
	public void CBLoadSceneAura4()			{ SceneManager.LoadScene("PolyAura4"); 		}
	public void CBLoadSceneBarrage()		{ SceneManager.LoadScene("PolyBarrage"); 	}
	public void CBLoadSceneBarrage2()		{ SceneManager.LoadScene("PolyBarrage2"); 	}
	public void CBLoadSceneChains()			{ SceneManager.LoadScene("PolyChains"); 	}
	public void CBLoadSceneChains2()		{ SceneManager.LoadScene("PolyChains2"); 	}
	public void CBLoadSceneCleave()			{ SceneManager.LoadScene("PolyCleave"); 	}
	public void CBLoadSceneCombat01()		{ SceneManager.LoadScene("PolyCombat01"); 	}
	public void CBLoadSceneCombat02()		{ SceneManager.LoadScene("PolyCombat02"); 	}
	public void CBLoadSceneCurses()			{ SceneManager.LoadScene("PolyCurses"); 	}
	public void CBLoadSceneDeath()			{ SceneManager.LoadScene("PolyDeath");	 	}
	public void CBLoadSceneEnchant()		{ SceneManager.LoadScene("PolyEnchant"); 	}
	public void CBLoadSceneExploMini()		{ SceneManager.LoadScene("PolyExploMini"); 	}
	public void CBLoadSceneGore()			{ SceneManager.LoadScene("PolyGore"); 		}
	public void CBLoadSceneHitscan()		{ SceneManager.LoadScene("PolyHitscan"); 	}
	public void CBLoadSceneNecromancy()		{ SceneManager.LoadScene("PolyNecromancy");	}
	public void CBLoadSceneNova()			{ SceneManager.LoadScene("PolyNova"); 		}
	public void CBLoadSceneOrbitalBeam()	{ SceneManager.LoadScene("PolyOrbitalBeam");}
	public void CBLoadSceneSpikes()			{ SceneManager.LoadScene("PolySpikes"); 	}
	public void CBLoadSceneSpikes2()		{ SceneManager.LoadScene("PolySpikes2"); 	}
	public void CBLoadSceneSpikes3()		{ SceneManager.LoadScene("PolySpikes3"); 	}
	public void CBLoadSceneSpikes4()		{ SceneManager.LoadScene("PolySpikes4"); 	}
	public void CBLoadSceneSurfaceDmg()		{ SceneManager.LoadScene("PolySurfaceDmg");	}
	public void CBLoadSceneSword()			{ SceneManager.LoadScene("PolySword"); 		}
	public void CBLoadSceneSwordTrail()		{ SceneManager.LoadScene("PolySwordTrail");	}
	
	//Environment Scenes
	
	public void ENVLoadSceneConfetti()		{ SceneManager.LoadScene("PolyConfetti"); 	}
	public void ENVLoadSceneEnvironment()	{ SceneManager.LoadScene("PolyEnvironment");}
	public void ENVLoadSceneFire()			{ SceneManager.LoadScene("PolyFire"); 		}
	public void ENVLoadSceneFire2()			{ SceneManager.LoadScene("PolyFire2"); 		}
	public void ENVLoadSceneFireflies()		{ SceneManager.LoadScene("PolyFireflies"); 	}
	public void ENVLoadSceneFireworks()		{ SceneManager.LoadScene("PolyFireworks"); 	}
	public void ENVLoadSceneLiquid()		{ SceneManager.LoadScene("PolyLiquid"); 	}
	public void ENVLoadSceneLiquid2()		{ SceneManager.LoadScene("PolyLiquid2"); 	}
	public void ENVLoadSceneRocks()			{ SceneManager.LoadScene("PolyRocks"); 		}
	public void ENVLoadSceneSparks()		{ SceneManager.LoadScene("PolySparks"); 	}
	public void ENVLoadSceneTornado()		{ SceneManager.LoadScene("PolyTornado"); 	}
	public void ENVLoadSceneWeather()		{ SceneManager.LoadScene("PolyWeather"); 	}
	
	//Interactive Scenes
	
	public void INTLoadSceneBeamUp()		{ SceneManager.LoadScene("PolyBeamUp"); 	}
	public void INTLoadSceneBlackHole()		{ SceneManager.LoadScene("PolyBlackHole"); 	}
	public void INTLoadSceneHeal()			{ SceneManager.LoadScene("PolyHeal"); 		}
	public void INTLoadSceneJets()			{ SceneManager.LoadScene("PolyJets"); 		}
	public void INTLoadSceneLoot()			{ SceneManager.LoadScene("PolyLoot"); 		}
	public void INTLoadScenePortal()		{ SceneManager.LoadScene("PolyPortal"); 	}
	public void INTLoadScenePortal2()		{ SceneManager.LoadScene("PolyPortal2"); 	}
	public void INTLoadScenePowerupIcon()	{ SceneManager.LoadScene("PolyPowerupIcon");}
	public void INTLoadSceneSpawn()			{ SceneManager.LoadScene("PolySpawn"); 		}
	public void INTLoadSceneTrails()		{ SceneManager.LoadScene("PolyTrails"); 	}
	public void INTLoadSceneTreasure()		{ SceneManager.LoadScene("PolyTreasure"); 	}
	public void INTLoadSceneTreasure2()		{ SceneManager.LoadScene("PolyTreasure2"); 	}
	public void INTLoadSceneZones()			{ SceneManager.LoadScene("PolyZones"); 		}
	
	 void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelectCom").GetComponent<Canvas> ().enabled = false;
         }
		 
		 else
		 {
             GameObject.Find("CanvasSceneSelectCom").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("CanvasMissiles").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasMissiles").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.H))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = true;
         }
     }
	 if(Input.GetKeyDown(KeyCode.M))
	 {
         GUIHide4 = !GUIHide4;
     
         if (GUIHide4)
		 {
             GameObject.Find("CanvasSceneSelectInt").GetComponent<Canvas> ().enabled = false;
         }
		 
		 else
		 {
             GameObject.Find("CanvasSceneSelectInt").GetComponent<Canvas> ().enabled = true;
         }
     }
	 if(Input.GetKeyDown(KeyCode.N))
	 {
         GUIHide5 = !GUIHide5;
     
         if (GUIHide5)
		 {
             GameObject.Find("CanvasSceneSelectEnv").GetComponent<Canvas> ().enabled = false;
         }
		 
		 else
		 {
             GameObject.Find("CanvasSceneSelectEnv").GetComponent<Canvas> ().enabled = true;
         }
     }
	}
}

}