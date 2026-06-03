import { useState } from "react";

interface Props{
    onNameSubmit: (name:string) => void
}

function NameInput({onNameSubmit} : Props){
    const[name,setName]= useState('');
    function handleSubmit() {
        if(name.trim() == '') return
        onNameSubmit(name.trim())
    }
    return (
         <div>
      <h2>Enter your name to start editing</h2>
      <input
        type="text"
        value={name}
        onChange={e => setName(e.target.value)}
        placeholder="Your name"
      />
      <button onClick={handleSubmit}>Join</button>
    </div>
    )
}
export default NameInput