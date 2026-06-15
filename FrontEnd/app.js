import { getRoomStrategy } from "./roomStrategy.js";

let rooms = [];

const template = document.getElementById("booking-row-template");
const tbody = document.getElementById("bookings-body");
const form = document.getElementById("booking-form");
const serviceTemplate = document.getElementById("service-row-template");
const serviceBody = document.getElementById("services-body");
const servicesEmpty = document.getElementById("services-empty");
const servicesTable = document.querySelector(".services__table");
const searchButton = document.getElementById("search-button");
const searchInput = document.getElementById("search-input");

form.addEventListener("submit", async (e) => {
    e.preventDefault();

    const roomId = document.getElementById("room-select").value;
    const guestIdsInput = document.getElementById("guest-ids").value;
    const startDate = document.getElementById("start-date").value;
    const endDate = document.getElementById("end-date").value;

    const guestIds = guestIdsInput
    .split(",")
    .map(id => parseInt(id.trim()));

    const selectRoom = rooms.find(r => r.id == roomId);

    const strategy = getRoomStrategy(selectRoom.type);
    
    if(!strategy.validate(selectRoom.capacity, guestIds.length)){
        alert("La cantidad de huespedes supera la capacidad");
        return;
    }

    if (!roomId) {
        alert("Seleccione una habitación");
        return;
    }

    const bookingData = {
        roomId: parseInt(roomId),
        startDate: new Date(startDate).toISOString(),
        endDate: new Date(endDate).toISOString(),
        numberOfGuests: guestIds.length,
        guestIds
    };

    console.log("BookingData:", bookingData);

    try {
        const request = await fetch("https://prototipo-hotel-production.up.railway.app/api/booking", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(bookingData)
        });

        alert(await request.text());

        loadBookings(); //refresca tabla

    } catch (error) {
        console.error(error);
        alert("Error creando reserva");
    }
});

function renderTable(bookings) {
    tbody.innerHTML = "";

    bookings.forEach(b => {
        const clone = template.content.cloneNode(true);

        clone.querySelector(".bookings__guests").textContent = (b.guests || []).join(", ");
        clone.querySelector(".bookings__room").textContent = `${b.roomType} (Cap: ${b.roomCapacity}, $${b.roomPrice})`;
        clone.querySelector(".bookings__start").textContent = formatDate(b.startDate);
        clone.querySelector(".bookings__end").textContent = formatDate(b.endDate);
        clone.querySelector(".bookings__status").textContent = b.status;

        const button = clone.querySelector(".bookings__checkin-btn");
        
        if(b.status === "IN_PROGRESS") {
            button.disabled = true;
            button.textContent = "Ya activo";
        }

        button.addEventListener("click", () => {
            checkIn(b.id);
        });

        tbody.appendChild(clone);
    });
}

function formatDate(date) {
    return new Date(date).toLocaleDateString();
}

searchButton.addEventListener("click", async (e) => {
    e.preventDefault();
    console.log("hola")
    loadBookings();
});

async function loadBookings() {
    let url;
    if(searchInput.value) {
        url = `https://prototipo-hotel-production.up.railway.app/api/booking/search?query=${searchInput.value}`;
    }
    else {
        url = `https://prototipo-hotel-production.up.railway.app/api/booking/active`;
    }        

    console.log("search input:", searchInput.value);
    console.log("url:", url)
    const request = await fetch(url);
    const bookings = await request.json();

    console.log("Bookings cargados:", bookings);

    renderTable(bookings);
}

async function loadRooms() {
    const request = await fetch("https://prototipo-hotel-production.up.railway.app/api/room");
    rooms = await request.json();

    const select = document.getElementById("room-select");

    rooms.forEach(room => {
        const option = document.createElement("option");

        option.value = room.id;
        option.textContent = `${room.type} (Cap: ${room.capacity})`;

        select.appendChild(option);
    });
}

async function checkIn(id) {
    if (!confirm("¿Realizar check-in?")) return;

    try {
        const request = await fetch(`https://prototipo-hotel-production.up.railway.app/api/booking/${id}/checkin`, {
            method: "POST"
        });

        const text = await request.text();
        alert(text);

        loadBookings(); //refresca la tabla

    } catch (error) {
        console.error(error);
        alert("Error en check-in");
    }
}

function renderServices(services) {
    console.log("Services:", services);
    console.log("Empty element:", servicesEmpty);

    serviceBody.innerHTML = "";

    if(!services || services.length === 0) {
        servicesEmpty.style.display = "block";
        return;
    }

    servicesEmpty.style.display = "none";

    services.forEach(s => {
        const clone = serviceTemplate.content.cloneNode(true);

        clone.querySelector(".services__name").textContent = s.name;
        clone.querySelector(".services__employee").textContent = s.employeeName;
        clone.querySelector(".services__phone").textContent = s.phone;

        serviceBody.appendChild(clone);
    })
}

async function loadServices() {
    const request = await fetch("https://prototipo-hotel-production.up.railway.app/api/service");
    const services = await request.json();

    renderServices(services);
}

console.log(searchInput);
loadServices();
loadRooms();
loadBookings();