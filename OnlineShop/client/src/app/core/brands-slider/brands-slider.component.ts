import { Component, ElementRef, ViewChild, AfterViewInit, Renderer2, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-brands-slider',
  templateUrl: './brands-slider.component.html',
  styleUrls: ['./brands-slider.component.scss']
})
export class BrandsSliderComponent {
  visibleLogos: Array<{ id: string; name: string; isActive: boolean, src: string }> = [];
  private logoChangeInterval!: Subscription;

  constructor(private renderer: Renderer2) { }

 
}
