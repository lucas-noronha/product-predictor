function getFutureMessage(item, predictedDate) {
  const dateStr = predictedDate.toLocaleDateString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
  });

  const templates = [
    `Talvez em breve você precise comprar ${item}.`,
    `Já se prepara: ${item} pode ser necessário por volta de ${dateStr}.`,
    `Bom ficar de olho em ${item}: a previsão é de compra perto de ${dateStr}.`,
    `${item} deve entrar na sua próxima lista em breve (por volta de ${dateStr}).`,
    `Parece que ${item} vai começar a faltar logo; considere comprar perto de ${dateStr}.`,
  ];

  const index = Math.floor(Math.random() * templates.length);
  return templates[index];
}

function getPastMessage(item, predictedDate) {
  const dateStr = predictedDate.toLocaleDateString("pt-BR", {
    day: "2-digit",
    month: "2-digit",
  });

  const templates = [
    `Pode ser que ${item} já tenha acabado (previsão era ${dateStr}).`,
    `Vale conferir o estoque de ${item}: a previsão de reposição era ${dateStr}.`,
    `${item} talvez esteja no fim — a previsão de compra passou em ${dateStr}.`,
    `Se ainda não comprou ${item}, talvez já esteja faltando desde ${dateStr}.`,
    `Hora de checar ${item}: a estimativa de nova compra era em ${dateStr}.`,
  ];

  const index = Math.floor(Math.random() * templates.length);
  return templates[index];
}

export function addMessagesToPredictions(predictions) {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  return (predictions || []).map((p) => {
    let message = null;
    let note = null;

    if (p.predictedDate) {
      const predicted = new Date(p.predictedDate);
      predicted.setHours(0, 0, 0, 0);

      const diffMs = predicted.getTime() - today.getTime();
      const diffDays = diffMs / (1000 * 60 * 60 * 24);

      if (Math.abs(diffDays) <= 3) {
        if (diffDays >= 0) {
          message = getFutureMessage(p.item, predicted);
        } else {
          message = getPastMessage(p.item, predicted);
        }

        note = `Previsão aproximada: ${predicted.toLocaleDateString("pt-BR")}.`;
      }
    }

    return {
      ...p,
      message,
      note,
    };
  });
}
