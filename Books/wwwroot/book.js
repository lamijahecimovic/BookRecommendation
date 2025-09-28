 async function ucitajKnjigu() {
            const params = new URLSearchParams(window.location.search);
    const knjigaId = params.get("knjigaId");
    if (!knjigaId) return;

    const res = await fetch(`/api/Knjiges/${knjigaId}`);
    if (!res.ok) {
        alert("Knjiga nije pronađena.");
    return;
            }

    const knjiga = await res.json();

    // Popuni podatke
    document.getElementById('book-title').textContent = knjiga.naslov;
    document.getElementById('book-author').textContent = knjiga.autor;
    if (knjiga.opis)
    document.getElementById('book-description').textContent = knjiga.opis;

    const imgUrl = knjiga.slikaUrl ? `/images/${knjiga.slikaUrl}` : '/images/default-book.jpg';
    document.getElementById('book-image').src = imgUrl;

    // Ako postoji PDF, prikaži dugme
    if (knjiga.pdfUrl) {
                const readBtn = document.getElementById('read-button');
    readBtn.style.display = 'inline-block';
                readBtn.addEventListener('click', () => {
        document.getElementById('pdfIframe').src = knjiga.pdfUrl;
    document.getElementById('pdfIframe').style.display = 'block';
    readBtn.style.display = 'none';
                });
            }
        }

    ucitajKnjigu();
    ucitajRecenzije();

    async function ucitajRecenzije() {
            const knjigaId = new URLSearchParams(window.location.search).get("knjigaId");

    const res = await fetch(`/api/Recenzijes/knjiga/${knjigaId}`);
    if (!res.ok) return;

    const recenzije = await res.json();
    const div = document.getElementById('lista-recenzija');
    div.innerHTML = '';

    if (recenzije.length === 0) {
        div.innerHTML = "<p>Nema recenzija za ovu knjigu.</p>";
    return;
            }

            recenzije.forEach(r => {
                const e = document.createElement('div');
    e.style.borderBottom = '1px solid #ccc';
    e.style.padding = '10px 0';
    e.innerHTML = `
    <strong>${r.korisnickoIme || 'Korisnik'}</strong>
    <div style="color: gold;">${'★'.repeat(r.ocjena)}${'☆'.repeat(5 - r.ocjena)}</div>
    <p>${r.komentar}</p>
    `;
    div.appendChild(e);
            });
}

let trenutnaOcjena = 0;

// Zvjezdice
const zvjezdice = document.getElementById('star-rating').children;
[...zvjezdice].forEach((zv, i) => {
    zv.addEventListener('mouseover', () => prikaziZvjezdice(i + 1));
    zv.addEventListener('mouseout', () => prikaziZvjezdice(trenutnaOcjena));
    zv.addEventListener('click', () => {
        trenutnaOcjena = i + 1;
        prikaziZvjezdice(trenutnaOcjena);
    });
});

function prikaziZvjezdice(ocjena) {
    [...zvjezdice].forEach((zv, i) => {
        zv.innerHTML = i < ocjena ? '&#9733;' : '&#9734;';
        zv.style.color = i < ocjena ? 'gold' : 'gray';
    });
}
async function posaljiRecenziju() {
    const komentar = document.getElementById('comment').value.trim();
    const knjigaId = new URLSearchParams(window.location.search).get("knjigaId");
    const korisnickoIme = localStorage.getItem('korisnickoIme');

    if (!korisnickoIme) {
        alert("Morate biti prijavljeni da ostavite recenziju.");
        return;
    }
    if (trenutnaOcjena === 0) {
        alert("Molimo, odaberite ocjenu zvjezdicama.");
        return;
    }
    // opcionalno, ako želiš obavezati komentar
    // if (komentar.length === 0) {
    //     alert("Molimo napišite komentar.");
    //     return;
    // }

    try {
        const korisnikRes = await fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`);
        if (!korisnikRes.ok) throw new Error("Ne mogu dohvatiti korisnika.");

        const korisnik = await korisnikRes.json();

        const dto = {
            korisnikId: korisnik.korisnikId,
            knjigaId: parseInt(knjigaId),
            komentar: komentar,
            ocjena: trenutnaOcjena
        };

        const res = await fetch('/api/Recenzijes', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        });

        if (res.ok) {
            alert("Hvala na recenziji!");
            document.getElementById('comment').value = '';
            trenutnaOcjena = 0;
            prikaziZvjezdice(0);
            await ucitajRecenzije(); // osvježi prikaz recenzija
        } else {
            const text = await res.text();
            alert("Greška prilikom slanja: " + text);
        }
    } catch (error) {
        alert("Greška: " + error.message);
    }
}