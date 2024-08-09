import { Duck } from "./Duck"

interface Props {
    duck: Duck
}

//curly backets use to access a property inside Props interface
export default function DuckItem({duck}: Props) {
    return (
        <div key={duck.name}>
            <span>{duck.name}</span>
            <button onClick={() => duck.makeSound(duck.name + " quack")}>Make sound</button>
        </div>
    )
}
