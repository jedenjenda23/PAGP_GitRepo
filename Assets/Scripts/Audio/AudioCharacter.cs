using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacter : MonoBehaviour {
    public GameObject Anim;                         // Animator refernce

    // Reference SurfaceTypů pro terén
    public SurfaceType Dirt;
    public SurfaceType Puddle;

    public AudioClip dashSound;                     // Dash sound
    public AudioSource source;
    private GC_PlayableHumanoidCharacter player;

    private Animator playerAnimator;
    private float currentRightFoot;
    private float lastFrameRightFoot;
    private float currentLeftFoot;
    private float lastFrameLeftFoot;

    private SurfaceType currentSurface;
    public GameObject terrain;                      //Stores the Terrain GameObject out of the Scene for use later.
    private Vector3 terrainPos;                     //Where are we on the Splatmap?
    private TerrainData terrainData;                //Lets us get to the Terrain's splatmap.
    public static int surfaceIndex = 0;

    private void Start()
    {
        playerAnimator = Anim.GetComponent<Animator>();
        player = GetComponent<GC_PlayableHumanoidCharacter>();
    }

    private void Update()
    {
        // Současná hodnota křivek pro porovnání
        currentRightFoot = playerAnimator.GetFloat("RightFoot");
        currentLeftFoot = playerAnimator.GetFloat("LeftFoot");

        

        // Pravá noha
        if (currentRightFoot < 0 && lastFrameRightFoot > 0)
        {
            PlayFootstep();
        }

        // Levá noha
        if (currentLeftFoot < 0 && lastFrameLeftFoot > 0)
        {
            PlayFootstep();
        }

        // Save current curve value for comparsion
        lastFrameRightFoot = currentRightFoot;
        lastFrameLeftFoot = currentLeftFoot;
    }

    private void PlayFootstep()
    {
        player.CheckSurfaceType();

        terrain = player.GetCurrentTerrain();

        // Na jaké textuře terénu hráč právě stojí?
        if (terrain != null && player.currentSurface.IsTerrain())
        {
            terrainData = terrain.GetComponent<Terrain>().terrainData;
            terrainPos = terrain.transform.position;
            surfaceIndex = GetMainTexture(transform.position);
            switch (terrainData.splatPrototypes[surfaceIndex].texture.name)
            {
                case "T_Dirt_01_A":
                    currentSurface = Dirt;
                    break;
                case "T_WaterPuddle_AS":
                    currentSurface = Puddle;
                    break;
                default:
                    currentSurface = Dirt;
                    break;

            }
        }

        // Pokud hráč nestojí na terénu, použij raycast
        //Debug.Log(player.currentSurface.IsTerrain());
        if (player.currentSurface != null)
        {
            if (player.currentSurface.IsTerrain() != true)
            {
                currentSurface = player.currentSurface;
            }
        }


        source.pitch = Random.Range(0.85f, 1.4f);
        source.PlayOneShot(currentSurface.GetRandomStepSound());
        GameObject newParticle = Instantiate(currentSurface.GetRandomStepParticle(), new Vector3(player.transform.position.x,player.transform.position.y-0.85f,player.transform.position.z),new Quaternion(0,0,0,0));
        Destroy(newParticle, 2f);
    }

    //Puts ALL TEXTURES from the Terrain into an array, represented by floats (0=first texture, 1=second texture, etc).
    private float[] GetTextureMix(Vector3 WorldPos)
    {
        if (terrain != null)
        {    //IS THERE A TERRAIN IN THE SCENE?
            // calculate which splat map cell the worldPos falls within
            int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
            // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1]; //turn splatmap data into float array
            for (int n = 0; n < cellMix.Length; n++)
            {
                cellMix[n] = splatmapData[0, 0, n];
            }
            return cellMix;
        }
        else return null; //THERE'S NO TERRAIN IN THE SCENE! DON'T DO THE ABOVE STUFF.
    }

    //Takes the "GetTextureMix" float array from above and returns the MOST DOMINANT texture at Player's position.
    private int GetMainTexture(Vector3 WorldPos)
    {
        if (terrain != null)
        {    //IS THERE A TERRAIN IN THE SCENE?
            float[] mix = GetTextureMix(WorldPos);
            float maxMix = 0;
            int maxIndex = 0;
            for (int n = 0; n < mix.Length; n++)
            {
                if (mix[n] > maxMix)
                {
                    maxIndex = n;
                    maxMix = mix[n];
                }
            }
            return maxIndex;
        }
        else return 0;    //THERE'S NO TERRAIN IN THE SCENE! DON'T DO THE ABOVE STUFF.
    }


    public void PlayDash()
    {
        //Debug.Log("Dash");
        source.pitch = 1.2f;
        source.PlayOneShot(dashSound);
    }
}
