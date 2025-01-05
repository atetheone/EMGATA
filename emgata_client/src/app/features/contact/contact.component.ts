// src/app/features/contact/contact.component.ts
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mx-auto px-4 py-8">
      <div class="max-w-3xl mx-auto">
        <h1 class="text-3xl font-bold mb-8 text-center">Contactez-nous</h1>
        
        <div class="grid grid-cols-1 md:grid-cols-2 gap-8">
          <!-- Coordonnées -->
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h2 class="card-title mb-4">Nos Coordonnées</h2>
              <div class="space-y-4">
                <div>
                  <h3 class="font-bold mb-2">Adresse</h3>
                  <p>
                    EMG Voitures<br/>
                    123 Avenue de la République<br/>
                    Dakar, Sénégal
                  </p>
                </div>
                
                <div>
                  <h3 class="font-bold mb-2">Téléphone</h3>
                  <p>+221 77 123 45 67</p>
                  <p>+221 77 987 65 43</p>
                </div>

                <div>
                  <h3 class="font-bold mb-2">Email</h3>
                  <p><a href="mailto:contact&#64;emg-voitures.com">contact[at]emg-voitures.com</a></p>
                </div>
              </div>
            </div>
          </div>

          <!-- Horaires -->
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h2 class="card-title mb-4">Horaires d'ouverture</h2>
              <div class="space-y-2">
                <p><span class="font-medium">Lundi - Vendredi:</span> 9h00 - 18h00</p>
                <p><span class="font-medium">Samedi:</span> 9h00 - 16h00</p>
                <p><span class="font-medium">Dimanche:</span> Fermé</p>
                
                <div class="divider"></div>
                
                <div class="mt-4">
                  <h3 class="font-bold mb-2">Note importante</h3>
                  <p class="text-sm">
                    Pour toute visite concernant un véhicule spécifique, 
                    nous vous recommandons de nous appeler à l'avance pour 
                    confirmer sa disponibilité.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Localisation -->
        <div class="mt-8">
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h2 class="card-title mb-4">Comment nous trouver</h2>
              <p class="mb-4">
                Nous sommes situés dans le centre-ville de Dakar, 
                à proximité de [point de repère connu]. 
                Notre showroom est facilement accessible en voiture 
                ou en transport en commun.
              </p>
              <div class="bg-base-200 p-4 rounded-lg">
                <h3 class="font-bold mb-2">Points de repère</h3>
                <ul class="list-disc list-inside space-y-1">
                  <li>À 5 minutes de la station de bus principale</li>
                  <li>Parking gratuit disponible sur place</li>
                  <li>En face de [point de repère]</li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `
})
export class ContactComponent { }