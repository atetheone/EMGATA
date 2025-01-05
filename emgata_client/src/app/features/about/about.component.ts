import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-about',
  imports: [CommonModule],
  template: `
    <div class="container mx-auto px-4 py-8">
      <div class="max-w-4xl mx-auto">
        <!-- En-tête -->
        <div class="text-center mb-12">
          <h1 class="text-4xl font-bold mb-4">À Propos d'EMG Voitures</h1>
          <p class="text-xl">Votre partenaire de confiance dans l'achat et la vente de véhicules d'occasion</p>
        </div>

        <!-- Notre Histoire -->
        <div class="card bg-base-100 shadow-xl mb-8">
          <div class="card-body">
            <h2 class="card-title text-2xl mb-4">Notre Histoire</h2>
            <p class="mb-4">
              Fondée en 2024, EMG Voitures est née d'une passion pour l'automobile et d'une vision : 
              offrir des véhicules d'occasion de qualité à des prix abordables au Sénégal.
            </p>
            <p>
              Notre expertise dans la sélection et la rénovation de véhicules nous permet 
              de garantir à nos clients des voitures fiables et performantes.
            </p>
          </div>
        </div>

        <!-- Nos Valeurs -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h3 class="card-title">Qualité</h3>
              <p>Nous sélectionnons rigoureusement chaque véhicule et assurons une remise en état professionnelle.</p>
            </div>
          </div>
          
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h3 class="card-title">Transparence</h3>
              <p>Nous fournissons toutes les informations nécessaires sur l'historique et l'état de nos véhicules.</p>
            </div>
          </div>
          
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <h3 class="card-title">Service Client</h3>
              <p>Notre équipe est à votre écoute pour vous accompagner dans votre projet d'achat ou de vente.</p>
            </div>
          </div>
        </div>


        <!-- Pourquoi Nous Choisir -->
        <div class="card bg-base-100 shadow-xl">
          <div class="card-body">
            <h2 class="card-title text-2xl mb-4">Pourquoi Nous Choisir ?</h2>
            <ul class="space-y-4">
              <li class="flex items-start">
                <span class="text-primary mr-2">✓</span>
                <div>
                  <strong>Expertise Technique</strong>
                  <p>Notre équipe de mécaniciens qualifiés inspecte minutieusement chaque véhicule.</p>
                </div>
              </li>
              <li class="flex items-start">
                <span class="text-primary mr-2">✓</span>
                <div>
                  <strong>Prix Compétitifs</strong>
                  <p>Nous proposons les meilleurs rapports qualité-prix du marché.</p>
                </div>
              </li>
              <li class="flex items-start">
                <span class="text-primary mr-2">✓</span>
                <div>
                  <strong>Service Après-Vente</strong>
                  <p>Nous assurons un suivi et un support après chaque vente.</p>
                </div>
              </li>
              <li class="flex items-start">
                <span class="text-primary mr-2">✓</span>
                <div>
                  <strong>Garantie</strong>
                  <p>Tous nos véhicules sont garantis pour votre tranquillité d'esprit.</p>
                </div>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  `
})
export class AboutComponent { }