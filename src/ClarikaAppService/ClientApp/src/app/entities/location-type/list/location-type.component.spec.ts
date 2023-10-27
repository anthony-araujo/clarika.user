import { ComponentFixture, TestBed } from "@angular/core/testing";
import { HttpHeaders, HttpResponse } from "@angular/common/http";
import { HttpClientTestingModule } from "@angular/common/http/testing";
import { of } from "rxjs";

import { LocationTypeService } from "../service/location-type.service";

import { LocationTypeComponent } from "./location-type.component";

describe("LocationType Management Component", () => {
  let comp: LocationTypeComponent;
  let fixture: ComponentFixture<LocationTypeComponent>;
  let service: LocationTypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [LocationTypeComponent],
    })
      .overrideTemplate(LocationTypeComponent, "")
      .compileComponents();

    fixture = TestBed.createComponent(LocationTypeComponent);
    comp = fixture.componentInstance;
    service = TestBed.inject(LocationTypeService);

    const headers = new HttpHeaders();
    jest.spyOn(service, "query").mockReturnValue(
      of(
        new HttpResponse({
          body: [{ id: 123 }],
          headers,
        })
      )
    );
  });

  it("Should call load all on init", () => {
    // WHEN
    comp.ngOnInit();

    // THEN
    expect(service.query).toHaveBeenCalled();
    expect(comp.locationTypes?.[0]).toEqual(
      expect.objectContaining({ id: 123 })
    );
  });
});
