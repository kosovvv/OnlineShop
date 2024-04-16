import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Type } from 'src/app/shared/models/type';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { TypeService } from 'src/app/shared/services/type-service';
import { ImageService } from 'src/app/shared/services/image.service';

@Component({
  selector: 'app-type-edit',
  templateUrl: './type-edit.component.html',
  styleUrls: ['./type-edit.component.scss']
})
export class TypeEditComponent implements OnInit {
  @Input() type!: Type
  @ViewChild("closeButton") closeButton!: ElementRef;
  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;

  typeForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: [''],
  })

  constructor(private fb: FormBuilder,private typeService: TypeService, private imageService: ImageService, private router: Router, private toastr: ToastrService) {}
  
  ngOnInit(): void {
    this.loadForm();
  }

  onSubmit() {
    const formData = this.GenerateFormData();

    this.typeService.editType(this.type.id, this.typeForm.value as any).pipe(
      switchMap((createProductResult) => {
        if (this.fileName && this.image) {
          return this.imageService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (uploadImageResult) => {
        this.toastr.success("Success editing catergory.")
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

  loadForm() {
    this.typeForm.get('name')?.patchValue(this.type.name);
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
