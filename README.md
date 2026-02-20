# UnityFiniteStateMachine

## Installation

There is several options to install this package:
- UPM
- directly in manifest

### Unity Package Manager

Open Unity Package Manager and go to **Add package from git URL...** and paste [https://github.com/ostrzolekpawel/UnityFiniteStateMachine.git?path=Assets/FiniteStateMachine](https://github.com/ostrzolekpawel/UnityFiniteStateMachine.git?path=Assets/FiniteStateMachine)

### Manifest
Add link to package from repository directly to manifest.json

**Example**
```json
{
    "dependencies": {
        // other packages
        // ...
        "com.osirisgames.fsm": "https://github.com/ostrzolekpawel/UnityFiniteStateMachine.git?path=Assets/FiniteStateMachine"
    }
}
```