using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCharacter : MonoBehaviour {
    public GameObject Anim;                 // Animator refernce,
    public SurfaceType Dirt;
    public SurfaceType Puddle;

    public AudioClip dashSound;             // Dash sound
    public AudioSource source;
    private GC_PlayableHumanoidCharacter player;

    private Animator playerAnimator;
    private float currentRightFoot;
    private float lastFrameRightFoot;
    private float currentLeftFoot;
    private float lastFrameLeftFoot;

    private SurfaceType currentSurface;
    public GameObject terrain;   //Stores the Terrain GameObject out of the Scene for use later.
    private Vector3 terrainPos;         //Where are we on the Splatmap?
    private TerrainData terrainData;    //Lets us get to the Terrain's splatmap.
    public static int surfaceIndex = 0;

    private void Start()
    {
        playerAnimator = Anim.GetComponent<Animator>();
        player = GetComponent<GC_PlayableHumanoidCharacter>();
        terrainData = terrain.GetComponent<Terrain>().terrainData;
        terrainPos = terrain.transform.position;


    }

    private void Update()
    {
        currentRightFoot = playerAnimator.GetFloat("RightFoot");
        currentLeftFoot = playerAnimator.GetFloat("LeftFoot");

        Debug.Log(GetMainTexture(transform.position));
        if (terrain != null)
        {    //IS THERE A TERRAIN IN THE SCENE?
            surfaceIndex = GetMainTexture(transform.position);
            //Not that it matters, but here we determine what position the Terrain Textures are in.
            //For example, If you added a grass texture, then a dirt, then a rock, you'd have grass=0, dirt=1, rock=2.
            switch (terrainData.splatPrototypes[surfaceIndex].texture.name)
            {
                case "T_Dirt_01_A":
                    currentSurface = Dirt;
                    break;
                case "T_WaterPuddle_AS":
                    currentSurface = Puddle;
                    break;

            }

            //Instead of messing around with numbers, we'll just check the texture's filename.
        }

        // Pravá noha
        if (currentRightFoot < 0 && lastFrameRightFoot > 0)
        {
            source.PlayOneShot(currentSurface.GetRandomStepSound());
            source.pitch = Random.Range(0.85f, 1.4f);
        }

        // Levá noha
        if (currentLeftFoot < 0 && lastFrameLeftFoot > 0)
        {
            source.PlayOneShot(currentSurface.GetRandomStepSound());
            source.pitch = Random.Range(0.85f, 1.4f);
        }

        lastFrameRightFoot = currentRightFoot;
        lastFrameLeftFoot = currentLeftFoot;
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
        Debug.Log("Dash");
        source.pitch = 1.2f;
        source.PlayOneShot(dashSound);
    }
}
