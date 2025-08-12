using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YuzuValen.Utils;
public class SetCursor : MonoBehaviourSingletonPersistent<SetCursor>
{
    public Texture2D cursorTexture;

    void Start()
    {

        //set the cursor origin to its centre. (default is upper left corner)
        Vector2 cursorOffset = new Vector2(cursorTexture.width / 8, cursorTexture.height / 8);

        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(cursorTexture, cursorOffset, CursorMode.Auto);
    }
}
