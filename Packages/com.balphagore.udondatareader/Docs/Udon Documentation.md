# Breaking down VRChat's Udon Implementation

## What is Udon?
VRChat claims Udon is a programming language built completely in-house by their developers. The name is claimed to represent their visual node-graph interface where you connect nodes with "noodles". While it is powered by node graphs, Udon was also built to accomodate other languages and channels in mind by having Udon read an "assembly" file asset rather than exclusively reading compiled data from the graph itself.

### Assembly!?
Not exactly assembly assembly, but node graphs and other languages are compiled into 'Udon Assembly' per se. For example, this is snippet of compiled Udon Assembly from the example UdonSharp script I wrote for our example scene. This assembly is valid and can be interpreted by Udon itself.

```
.data_start
    .export syncdString
    .sync syncdString, none
    .sync privateSyncdString, none
    __refl_typeid: %SystemInt64, null
    __refl_typename: %SystemString, null
    __intnl_returnJump_SystemUInt32_0: %SystemUInt32, null
.data_end
.code_start
# 
# Balphagore.UdonDataReader.Runtime.UdonDataReaderUSharp.PublicMethod(string)
# 
        PUSH, __const_SystemString_0
        PUSH, __0_name__param
        PUSH, __0___0_PublicMethod__ret
        EXTERN, "SystemString.__op_Addition__SystemString_SystemString__SystemString"
        PUSH, __intnl_returnJump_SystemUInt32_0
        COPY
        JUMP_INDIRECT, __intnl_returnJump_SystemUInt32_0
        PUSH, __intnl_returnJump_SystemUInt32_0
        COPY
        JUMP_INDIRECT, __intnl_returnJump_SystemUInt32_0
    .export PublicIntMethod
    PublicIntMethod:
        PUSH, __const_SystemUInt32_0
.code_end
```

## How does Udon Work?
By default, VRChat will not run unapproved behaviours with the exception of the Udon VM including worlds that are authorized by VRChat to execute mono behaviours.

This is where Udon comes in and works as the middleman between the developer and the game itself. Udon is the gatekeeper that keeps developers from running malicious code on your game while executing whitelisted commands against the game engine on our behalf.

Udon also packs a VM(Virtual Machine) as a middleware that runs within the VRChat game client and the Unity Editor when you utilize VRChat's "Client Sim" implementation from their SDK to execute Udon behaviours in your scene.

## Anatomy of a Udon Program

### Variables
* Private variables do not have an exported symbol.
* Public variables do have an exported symbol and a normal symbol.

### Methods
* Private methods are not visible.
* Methods with a return type will find an accomodating variable for its return value.
    * For example, method "PublicDataTokenMethod" will have its return type and data propagated to the variable "__0_PublicDataTokenMethod__ret"


## Externs
Take a look at Phasedragon's post about the Udon VM.
```
An extern is anything that is ran outside the virtual machine. When you hear virtual machine you may think something like windows running in a window… No. The udon virtual machine is a deceptively simple bit of code that just runs assembly. It only natively supports a very small handful of operations like setting variables, getting variables, jumping around to different code, and externs. This means that basically any operation from basic addition to raycasts are externs. Udon itself has no idea what addition is, all it knows is that it can ask unity for the answer.

One of the reasons (and certainly not the only reason) why udon is slow is that it doesn’t trust externs. When udon receives back the results of an extern, it has to run a security check to make sure the object isn’t part of the UI or players or anything else that’s protected. The security check can sometimes be more expensive than the function itself. Though if it’s a primitive type like a bool or float, it can skip a large part of that but it still has to check it’s type.

In most cases, the cost of an extern is generally just something that has to be accepted. The performance has and will continue to increase over time.

However, it’s still worth knowing about externs because it highlights the usefulness of one particular thing that you can actually do: caching. If you’re working with an object that you receive from an extern, it is a bit cheaper to store it in a variable inside udon rather than doing an extern to get the same thing over and over again. If it’s stored as a variable in udon, udon knows it can already be trusted so it does not perform the security check every time it’s accessed.

The most common example is Networking.LocalPlayer. that is an extern, and it would be cheaper to just do that once on start and store it in a variable somewhere.
```

And more to come...