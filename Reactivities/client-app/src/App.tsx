import './App.css'
import { ducks } from './demo'

function App() {
  return (
    <div>
      <h1>Reactivities</h1>
      {ducks.map(duck => (
        <div key={duck.name}>
          <span>{duck.name}</span>
          <button onClick={() => duck.makeSound(duck.name + " quack")}></button>
        </div>
      ))}
    </div>

  )
}

export default App
