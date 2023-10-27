import { Component, OnInit } from "@angular/core";
import { HttpResponse } from "@angular/common/http";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { finalize } from "rxjs/operators";

import { ILocationType, LocationType } from "../location-type.model";
import { LocationTypeService } from "../service/location-type.service";

@Component({
  selector: "jhi-location-type-update",
  templateUrl: "./location-type-update.component.html",
})
export class LocationTypeUpdateComponent implements OnInit {
  isSaving = false;

  editForm = this.fb.group({
    id: [],
    name: [null, [Validators.required]],
  });

  constructor(
    protected locationTypeService: LocationTypeService,
    protected activatedRoute: ActivatedRoute,
    protected fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ locationType }) => {
      this.updateForm(locationType);
    });
  }

  previousState(): void {
    window.history.back();
  }

  save(): void {
    this.isSaving = true;
    const locationType = this.createFromForm();
    if (locationType.id !== undefined) {
      this.subscribeToSaveResponse(
        this.locationTypeService.update(locationType)
      );
    } else {
      this.subscribeToSaveResponse(
        this.locationTypeService.create(locationType)
      );
    }
  }

  protected subscribeToSaveResponse(
    result: Observable<HttpResponse<ILocationType>>
  ): void {
    result.pipe(finalize(() => this.onSaveFinalize())).subscribe({
      next: () => this.onSaveSuccess(),
      error: () => this.onSaveError(),
    });
  }

  protected onSaveSuccess(): void {
    this.previousState();
  }

  protected onSaveError(): void {
    // Api for inheritance.
  }

  protected onSaveFinalize(): void {
    this.isSaving = false;
  }

  protected updateForm(locationType: ILocationType): void {
    this.editForm.patchValue({
      id: locationType.id,
      name: locationType.name,
    });
  }

  protected createFromForm(): ILocationType {
    return {
      ...new LocationType(),
      id: this.editForm.get(["id"])!.value,
      name: this.editForm.get(["name"])!.value,
    };
  }
}
