import { Component, ElementRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { TypeService } from 'src/app/core/services/type-service';
import { ImageService } from 'src/app/core/services/image.service';

@Component({
  selector: 'app-type-create',
  templateUrl: './type-create.component.html',
  styleUrls: ['./type-create.component.scss']
})
export class TypeCreateComponent {
  @ViewChild("closeButton") closeButton!: ElementRef;
  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;

  typeForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: ['', Validators.required],
  })

  constructor(private fb: FormBuilder, private imageService: ImageService ,private typeService: TypeService, private router: Router, private toastr: ToastrService) {}

  onSubmit() {
    const formData = this.GenerateFormData();

    this.typeService.createType(this.typeForm.value as any).pipe(
      switchMap((type) => {
        if (this.fileName && this.image) {
          return this.imageService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (image) => {
        this.toastr.success("Success creating type.")
        this.closeButton.nativeElement.click();
      },
      error: (error) => this.toastr.error(error)
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.fileName = file.name;
      this.image = file;
      this.typeForm.get('pictureUrl')?.setValue(`api/images/types/${this.image.name}`)
    }
  }

  private GenerateFormData(): FormData{
    const formData = new FormData();

    if (this.fileName && this.image) {
      formData.append('Name', this.fileName);
      formData.append('Image', this.image, this.image.name);
    }
    return formData;
  }
}
