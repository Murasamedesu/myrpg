using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
using UnityEditorInternal;
using Services;
using System.IO.IsolatedStorage;

public class PlayerInputController : MonoBehaviour
{
    public Rigidbody rb;
    public Character character;

    public float rotateSpeed = 2.0f;
    public float turnAngle = 10;
    public int speed;

    public EntityController entityController;

    public bool OnAir = false;

    SkillBridge.Message.CharacterState state;


    void Start()
    {
        state = SkillBridge.Message.CharacterState.Idle;
        if(character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(character == null)
        {
            return;
        }
        //if (InputManager.Instance != null && InputManager.Instance.IsInputMode) return;
        
        float v = Input.GetAxis("Vertical");
        if(v > 0.01)
        {
            if(state != SkillBridge.Message.CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveForward();
                SendEntityEvent(EntityEvent.MoveFwd);
            }
            rb.velocity = rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (character.speed + 9.81f) / 100f;
        }
        else if(v < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = CharacterState.Move;
                character.MoveBack();
                SendEntityEvent(EntityEvent.MoveBack);
            }
            rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = CharacterState.Idle;
                rb.velocity = Vector3.zero;
                character.Stop();
                SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendEntityEvent(EntityEvent.Jump);
        }

        float h = Input.GetAxis("Horizontal");
        if(h < -0.1f || h > 0.1f)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }

        }

        //Debug.LogFormat("velocity {0}", this.rb.velocity.magnitude);

    }

    Vector3 lastPos;
    float lastSync = 0;

    private void LateUpdate()
    {
        if (this.character == null) return;

        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (logicOffset > 100)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;
    }


    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        if (entityController != null)
        {
            entityController.OnEntityEvent(entityEvent, param);
        }
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData, param);
    }


}
