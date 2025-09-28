async function ucitajPreporuke() {
    try {
        const korisnickoIme = localStorage.getItem('korisnickoIme');
        console.log("Korisnicko ime:", korisnickoIme);
        if (!korisnickoIme) return;

        const korisnikRes = await fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`);
        console.log("Status Auth:", korisnikRes.status);
        if (!korisnikRes.ok) throw new Error("Neuspješan dohvat korisnika");

        const korisnik = await korisnikRes.json();
        console.log("Korisnik:", korisnik);
        const korisnikId = korisnik.korisnikId;

        const preporukeRes = await fetch(`/api/Recommendations/${korisnikId}`); 



        console.log("Status preporuke:", preporukeRes.status);
        if (!preporukeRes.ok) throw new Error("Neuspješan dohvat preporuka");

        const preporuke = await preporukeRes.json();
        console.log("Preporuke:", preporuke);


        console.log("Status preporuke:", preporukeRes.status);
        if (!preporukeRes.ok) throw new Error("Neuspješan dohvat preporuka");



        const container = document.getElementById('recommended-list');
        container.innerHTML = '';

        if (preporuke.length === 0) {
            container.innerHTML = '<p>Trenutno nema preporuka za tebe.</p>';
            return;
        }

        preporuke.forEach(rec => {
            const knjiga = rec.knjiga;
            const imageName = knjiga.slikaUrl ? knjiga.slikaUrl : 'default-book.jpg';
            const slikaPutanja = `/images/${imageName}`;

            const card = document.createElement('div');
            card.className = 'book-card';
            card.style.cursor = 'pointer';
            card.addEventListener('click', () => {
                window.location.href = `book.html?knjigaId=${knjiga.knjigaId}`;
            });

            card.innerHTML = `
                <img src="${slikaPutanja}" alt="Naslovna strana: ${knjiga.naslov}" onerror="this.onerror=null; this.src='/images/default-book.jpg'">
                <div class="info">
                    <h4>${knjiga.naslov}</h4>
                    <p>${knjiga.autor}</p>
                    <p style="font-size: 13px; color: #b8860b;">Predviđena ocjena: ${rec.predvidjenaOcjena.toFixed(2)}</p>
                </div>
            `;

            container.appendChild(card);
        });

    } catch (err) {
        console.error("Greška pri učitavanju preporuka:", err);
    }
}



window.onload = async function () {
    await ucitajKnjige();

    const korisnickoIme = localStorage.getItem('korisnickoIme');
    if (korisnickoIme) {
        const korisnikRes = await fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`);
        if (korisnikRes.ok) {
            const korisnik = await korisnikRes.json();
            const korisnikId = korisnik.korisnikId;

            await fetch(`/api/Recommendations/${korisnikId}`, { method: 'POST' });

            
            await ucitajPreporuke();
        }
    }
};

const korisnickoIme = localStorage.getItem('korisnickoIme');
if (!korisnickoIme) {
    window.location.href = "login.html"; 
}
function openSidebar() {
    document.getElementById("sidebar").style.width = "300px";


    const korisnickoIme = localStorage.getItem('korisnickoIme');
    if (!korisnickoIme) return;


    fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`)
        .then(res => res.json())
        .then(data => {
            const content = `
                            <p><strong>Ime:</strong> ${data.ime}</p>
                            <p><strong>Prezime:</strong> ${data.prezime}</p>
                            <p><strong>Email:</strong> ${data.email}</p>
                                                                    `;
            document.querySelector('.sidebar-content').innerHTML = content;
        })
        .catch(err => {
            console.error("Greška pri dohvaćanju profila:", err);
            document.querySelector('.sidebar-content').innerHTML = "<p>Greška pri učitavanju profila.</p>";
        });
}

function closeSidebar() {
    document.getElementById("sidebar").style.width = "0";
}


document.querySelectorAll('nav a').forEach(link => {
    if (link.textContent.trim().toLowerCase() === 'profil') {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            openSidebar();
        });
    }
});
function openLogoutModal() {
    document.getElementById('logoutModal').style.display = 'flex';
}

function closeLogoutModal() {
    document.getElementById('logoutModal').style.display = 'none';
}

function confirmLogout() {

    localStorage.clear();
    window.location.href = 'login.html';
}
async function ucitajKnjige() {
    try {
        const response = await fetch('/api/Knjiges');

        if (!response.ok) {
            throw new Error('Greška u odgovoru sa servera.');
        }

        const knjige = await response.json();

        const container = document.getElementById('book-list');
        container.innerHTML = '';

        knjige.forEach(knjiga => {
            const imageName = knjiga.slikaUrl ? knjiga.slikaUrl : 'default-book.jpg';
            const slikaPutanja = `/images/${imageName}`;

            const card = document.createElement('div');
            card.className = 'book-card';
            card.style.cursor = 'pointer';
            card.addEventListener('click', () => {
                window.location.href = `book.html?knjigaId=${knjiga.knjigaId}`;
            });

            card.innerHTML = `
                                    <button class="favorite-btn" title="Dodaj u favorite">&#9734;</button>
                                    <img src="${slikaPutanja}"
                                         onerror="this.onerror=null; this.src='/images/default-book.jpg'"
                                         alt="Naslovna strana: ${knjiga.naslov}">
                                    <div class="info">
                                        <h4>${knjiga.naslov}</h4>
                                        <p>${knjiga.autor}</p>
                                    </div>
                                  `;


            const favoriteBtn = card.querySelector('.favorite-btn');
            const favorites = JSON.parse(localStorage.getItem('favoriteBooks') || '[]');

            if (favorites.includes(knjiga.knjigaId)) {
                favoriteBtn.classList.add('favorited');
                favoriteBtn.innerHTML = '&#9733;';
            }

            favoriteBtn.addEventListener('click', async (e) => {
                e.stopPropagation(); 

                const korisnickoIme = localStorage.getItem('korisnickoIme');
                if (!korisnickoIme) {
                    alert("Morate biti prijavljeni.");
                    return;
                }

               
                const korisnikRes = await fetch(`/api/Auth/me?korisnickoIme=${korisnickoIme}`);
                const korisnik = await korisnikRes.json();

                const dto = {
                    korisnikId: korisnik.korisnikId,
                    knjigaId: knjiga.knjigaId
                };

                
                if (favoriteBtn.classList.contains('favorited')) {
                    
                    
                    return;
                }

                const res = await fetch('/api/Favoritis/dodaj', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(dto)
                });

                if (res.ok) {
                    favoriteBtn.classList.add('favorited');
                    favoriteBtn.innerHTML = '&#9733;';
                } else {
                    const err = await res.text();
                    alert("Greška: " + err);
                }
            });


            container.appendChild(card);
        });
    } catch (err) {
        console.error('Greška prilikom učitavanja knjiga:', err);
    }
}