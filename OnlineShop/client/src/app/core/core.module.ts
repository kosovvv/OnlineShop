import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from '../shared/nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { TestErrorComponent } from '../shared/test-error/test-error.component';
import { NotFoundComponent } from '../shared/not-found/not-found.component';
import { ServerErrorComponent } from '../shared/server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeaderComponent } from '../shared/section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';
import { NgxSpinner, NgxSpinnerModule } from 'ngx-spinner';
import { SharedModule } from '../shared/shared.module';
import { BrandsSliderComponent } from '../shared/brands-slider/brands-slider.component';

@NgModule({
  declarations: [
    NavBarComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    SectionHeaderComponent,
    BrandsSliderComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    BreadcrumbModule,
    NgxSpinnerModule,
    SharedModule
  ],
  exports: [
    NavBarComponent,
    SectionHeaderComponent,
    NgxSpinnerModule,
    BrandsSliderComponent
  ]
})
export class CoreModule { }
