import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { RouterTestingModule } from "@angular/router/testing";
import { of, Subject, from } from "rxjs";

import { LocationTypeService } from "../service/location-type.service";
import { ILocationType, LocationType } from "../location-type.model";

import { LocationTypeUpdateComponent } from "./location-type-update.component";

describe("LocationType Management Update Component", () => {
  let comp: LocationTypeUpdateComponent;
  let fixture: ComponentFixture<LocationTypeUpdateComponent>;
  let activatedRoute: ActivatedRoute;
  let locationTypeService: LocationTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule.withRoutes([])],
      declarations: [LocationTypeUpdateComponent],
      providers: [
        FormBuilder,
        {
          provide: ActivatedRoute,
          useValue: {
            params: from([{}]),
          },
        },
      ],
    })
      .overrideTemplate(LocationTypeUpdateComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(LocationTypeUpdateComponent);
    activatedRoute = TestBed.inject(ActivatedRoute);
    locationTypeService = TestBed.inject(LocationTypeService);

    comp = fixture.componentInstance;
  });

  describe("ngOnInit", () => {
    it("Should update editForm", () => {
      const locationType: ILocationType = { id: 456 };

      activatedRoute.data = of({ locationType });
      comp.ngOnInit();

      expect(comp.editForm.value).toEqual(
        expect.objectContaining(locationType)
      );
    });
  });

  describe("save", () => {
    it("Should call update service on save for existing entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<LocationType>>();
      const locationType = { id: 123 };
      jest.spyOn(locationTypeService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ locationType });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: locationType }));
      saveSubject.complete();

      // THEN
      expect(comp.previousState).toHaveBeenCalled();
      expect(locationTypeService.update).toHaveBeenCalledWith(locationType);
      expect(comp.isSaving).toEqual(false);
    });

    it("Should call create service on save for new entity", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<LocationType>>();
      const locationType = new LocationType();
      jest.spyOn(locationTypeService, "create").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ locationType });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.next(new HttpResponse({ body: locationType }));
      saveSubject.complete();

      // THEN
      expect(locationTypeService.create).toHaveBeenCalledWith(locationType);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).toHaveBeenCalled();
    });

    it("Should set isSaving to false on error", () => {
      // GIVEN
      const saveSubject = new Subject<HttpResponse<LocationType>>();
      const locationType = { id: 123 };
      jest.spyOn(locationTypeService, "update").mockReturnValue(saveSubject);
      jest.spyOn(comp, "previousState");
      activatedRoute.data = of({ locationType });
      comp.ngOnInit();

      // WHEN
      comp.save();
      expect(comp.isSaving).toEqual(true);
      saveSubject.error("This is an error!");

      // THEN
      expect(locationTypeService.update).toHaveBeenCalledWith(locationType);
      expect(comp.isSaving).toEqual(false);
      expect(comp.previousState).not.toHaveBeenCalled();
    });
  });
});
