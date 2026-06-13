export class RoomStrategy {
    validate(capacity, guests) {
        throw new Error("Metodo no implementado");
    }
}

export class SimpleRoomStrategy extends RoomStrategy {
    validate(capacity, guests) {
        return guests <= capacity;
    }
}

export class SuiteRoomStrategy extends RoomStrategy {
    validate(capacity, guests) {
        return guests <= capacity;
    }
}

export class TwinRoomStrategy extends RoomStrategy {
    validate(capacity, guests) {
        return guests <= capacity;
    }
}

export class DoubleRoomStrategy extends RoomStrategy {
    validate(capacity, guests) {
        return guests <= capacity;
    }
}

export function getRoomStrategy(type) {
    switch(type) {
        case "Simple":
            return new SimpleRoomStrategy();
        case "Suite":
            return new SuiteRoomStrategy();
        case "Doble con camas individuales":
            return new TwinRoomStrategy();
        case "Doble matrimonial":
            return new DoubleRoomStrategy();
        default:
            throw new Error("Tipo de habitación no soportada");
    }
}