export interface Duck {
    name : string;
    numLegs: number;
    makeSound: (sound : string) => void;
}

const Duck1 : Duck = 
{
    name: "huey",
    numLegs: 4,
    makeSound: (sound : string) => console.log(sound)
}

const Duck2 : Duck = 
{
    name: "duey",
    numLegs: 2,
    makeSound: (sound : string) => console.log(sound)
}

Duck1.makeSound("quack quack");
Duck2.makeSound("quack quack");

export const ducks = [Duck1,Duck2]