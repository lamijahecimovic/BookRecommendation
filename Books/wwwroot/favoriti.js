async function ucitajFavorite() {
    const korisnickoIme = localStorage.getItem('korisnickoIme');
    if (!korisnickoIme) {
        window.location.href = "login.html";
        return;
    }

    const korisnikRes = await fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`);
    const korisnik = await korisnikRes.json();

    const res = await fetch(`/api/Favoritis/korisnik/${korisnik.korisnikId}`);
    const knjige = await res.json();

    const container = document.getElementById('favoriti-lista');
    container.innerHTML = '';
    if (knjige.length === 0) {
        container.innerHTML = '<p class="prazno">Nemate nijednu sačuvanu knjigu.</p>';
        return;
    }

    knjige.forEach(knjiga => {
        const img = knjiga.slikaUrl ? `/images/${knjiga.slikaUrl}` : '/images/default-book.jpg';

        const div = document.createElement('div');
        div.className = 'book-card';
        div.innerHTML = `
                                <img src="${img}" alt="${knjiga.naslov}">
                                <div class="info">
                                    <h4>${knjiga.naslov}</h4>
                                    <p>${knjiga.autor}</p>
                                <button class="delete-btn"><i class="fas fa-trash"></i></button>

                                <i class="fas fa-trash-alt"></i>
                            </button>
                                </div>
                            `;
        const btn = div.querySelector('.delete-btn');
        btn.addEventListener('click', async (e) => {
            e.stopPropagation();

            if (!confirm("Da li želite ukloniti ovu knjigu iz favorita?")) return;

            try {
                const deleteRes = await fetch(`/api/Favoritis/ukloni?korisnikId=${korisnik.korisnikId}&knjigaId=${knjiga.knjigaId}`, {
                    method: 'DELETE'
                });

                if (deleteRes.ok) {
                    div.remove(); 
                    if (container.children.length === 0) {
                        container.innerHTML = '<p class="prazno">Nemate nijednu sačuvanu knjigu.</p>';
                    }
                } else {
                    alert("Greška pri uklanjanju iz favorita.");
                }
            } catch (err) {
                console.error("Greška:", err);
            }
        });
        container.appendChild(div);
    });
}

window.onload = ucitajFavorite;