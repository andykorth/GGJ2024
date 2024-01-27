
# FYI: abandoned this and switched to OneJS

## Structure

- Stack
  - Screen
    - Window
      - TODO
      - Card?

## Naming Convention (suggested)

| type           | C# wrapper | affix | notes                       |
|----------------|------------|-------|-----------------------------|
| Stack          |            |       |                             |
| Screen         |            | S_    |                             |
| Window         |            | W_    |                             |
| Cell           | CEll       | C_    | multiple instance (pooling) | 
| Visual Element | VEl        | V_    | optional                    |
| Label          | LabEl      | L_    |                             |
| Button         | Btn        | B_    |                             |
| Radio button   | RadioBtn   | R_    |                             |
| Radio group    | RadioGrp   | G_    |                             |

sElect
panEl

## Misc

- no way to clone part of VisualTreeAsset" https://forum.unity.com/threads/how-to-make-a-clone-copy-of-a-visualelement.723782/#post-8867631

## TODO

- [ ] Could there be a generic version of GluWindow?
  - give name of "Window" element, loads using parent Screen Document
  - traverses tree and adds references, all or some based on naming convention?
  - how to then update from C#/game side?