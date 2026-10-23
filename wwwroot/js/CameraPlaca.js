const tPrint = document.getElementById("print")

const video = document.getElementById("cameraVideo");
const canvas = document.getElementById("cameraCanvas");
const overlayCanvas = document.getElementById("overlayCanvas");

async function Camera() {
    try {
        const stream = await navigator.mediaDevices.getUserMedia({
            video: { facingMode: "environment" }, //para usar a câmera traseira do telefone: video: { facingMode: { exact: "environment" } }
            audio: false
        });

        video.srcObject = stream;

        console.log("Câmera iniciada com sucesso");
    }
    catch (err) {
        console.error("Erro câmera:", err.name, err.message);
        alert("Erro ao abrir câmera: " + err.name + " - " + err.message);
    }
}
Camera()

const url = "https://localhost:7191/"
const overlayCtx = overlayCanvas.getContext("2d");

async function Enviar() {

    tPrint.textContent = Date()

    const context = canvas.getContext('2d');

    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;

    context.drawImage(video, 0, 0, canvas.width, canvas.height);

    canvas.toBlob(async (blob) => {
        try {
            //metrica.capturaConcluida();
            if (!blob) throw new Error('Nao foi possivel capturar a imagem.');

            const formData = new FormData();
            formData.append("frame", blob, "frame.jpg");
            etapaAtual = 'leitura';
            
            const response = await fetch(url + 'LerPlaca/Enviar', {
                method: 'POST',
                body: formData
            });

            const data = await response.json();
            //placa,x1,x2,y1,y2
            if (!data) {
                return;
            }
            data.placa = data.placa.toUpperCase();

            tPrint.textContent = data.placa + data.x1 + data.x2 + data.y1 + data.y2

            //desenhar quadrado
            overlayCanvas.width = video.videoWidth;
            overlayCanvas.height = video.videoHeight;

            overlayCtx.clearRect(0, 0, overlayCanvas.width, overlayCanvas.height);

            const x = data.x1;
            const y = data.y1;
            const w = data.x2 - data.x1;
            const h = data.y2 - data.y1;

            overlayCtx.lineWidth = 3;
            overlayCtx.strokeStyle = "lime";
            overlayCtx.strokeRect(x, y, w, h);

            overlayCtx.fillStyle = "lime";
            overlayCtx.font = "20px Arial";
            overlayCtx.fillText(data.placa || "Detectando...", x, y - 10);

            // const data = await response.json();
            // if (!response.ok || data.sucesso === false)
            //     throw new Error(data.mensagem || 'Falha no leitor de placas.');

            // desenharOverlay(data);

            // if (data.placa) {
            //     placaLida = true;

            //     if (btnLerPlaca) {
            //         btnLerPlaca.textContent = "Registrando entrada...";
            //     }

            //     etapaAtual = 'registro';
            //     await registrarEntrada(data.placa, undefined, metrica);
            //     resultadoMetrica = 'success';

            //     if (btnLerPlaca) {
            //         btnLerPlaca.textContent = "Entrada confirmada";
            //     }
            // } else {
            //     resultadoMetrica = 'no_plate';
            //     if (btnLerPlaca) {
            //         btnLerPlaca.disabled = false;
            //         btnLerPlaca.textContent = "Confirmar Entrada";
            //     }

            //     exibirToastErro("Não foi possível ler a placa. Tente novamente.");
            // }
        }
        catch (err) {
            console.error(`Erro na etapa ${etapaAtual}:`, err);

            placaLida = false;

            if (btnLerPlaca) {
                btnLerPlaca.disabled = false;
                btnLerPlaca.textContent = "Confirmar Entrada";
            }

            const nomeEtapa = {
                captura: 'Captura da imagem',
                leitura: 'Leitura da placa',
                registro: 'Registro da entrada'
            }[etapaAtual];
            const detalhe = err instanceof Error && err.message
                ? err.message
                : 'Falha não identificada.';
            exibirToastErro(`${nomeEtapa}: ${detalhe}`);
        }
        finally {
            processando = false;
            metrica.concluir(resultadoMetrica);
        }

    }, 'image/jpeg');


};

document.getElementById("enviar").addEventListener("click", Enviar)