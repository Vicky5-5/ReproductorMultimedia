//Obtenemos el canvas y su contexto
const canvas = document.getElementById("mouseTrailCanvas"); //Seleccionamos el id del canva
const ctx = canvas.getContext("2d"); //obtiene el contexto 2D para dibujar

//Ajustamos el tamaño del canvas al tamaño de la ventana
canvas.width = window.innerWidth; //ancho de la pantalla
canvas.height = window.innerHeight; //alto de la pantalla

//Redimensionamos el canvas si cambia el tamaño de la pantalla
window.addEventListener("resize", () => {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
});

//Creamos las partículas al mover el ratón
let particles = []; //Array que guarda cada partícula

document.addEventListener("mousemove", (e) => {
    //Creamos varias partículas por movimiento para mayor efecto
    for (let i = 0; i < 3; i++) {
        particles.push({
            x: e.clientX + Math.random() * 10 - 5, //Coordenada x con leve dispersión
            y: e.clientY + Math.random() * 10 - 5, //Coordenada y con leve dispersión
            radius: Math.random() * 5 + 2, //Para el radio aleatorio entre 2 y 7
            alpha: 1, //La opacidad incial, que al principio es visible
            color: `rgba(121, 131, 255, 1)` //color de las partículas
        });
    }
});

//Dibujamos las partículas
function draw() {
    ctx.clearRect(0, 0, canvas.width, canvas.height); //Limpia cada frame para evitar que las partículas se acumulen

    //Recorremos cada partícula y la dibujamos (de atrás hacia adelante para evitar errores al eliminar)
    for (let i = particles.length - 1; i >= 0; i--) {
        const p = particles[i];
        ctx.beginPath();
        ctx.arc(p.x, p.y, p.radius, 0, Math.PI * 2);
        ctx.fillStyle = p.color.replace("1)", `${p.alpha})`); //Usa el color con la opacidad actual
        ctx.fill();

        //Animamos la desaparición de las partículas
        p.alpha -= 0.02;
        p.radius -= 0.1;

        //Eliminamos la partícula si ya no es visible
        if (p.alpha <= 0 || p.radius <= 0) {
            particles.splice(i, 1);
        }
    }

    //llama a draw() de nuevo en el siguiente frame
    requestAnimationFrame(draw);
}

draw(); //Iniciamos la animación
